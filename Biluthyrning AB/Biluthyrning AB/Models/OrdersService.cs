﻿using Biluthyrning_AB.Models.Data;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.Repositorys;
using Biluthyrning_AB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models
{
    public class OrdersService
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly CustomersService customersService;
        private readonly EventsService eventsService;
        private readonly CarsService carsService;

        public OrdersService(IOrdersRepository ordersRepository, CustomersService customersService, EventsService eventsService, CarsService carsService)
        {
            this.ordersRepository = ordersRepository;
            this.customersService = customersService;
            this.eventsService = eventsService;
            this.carsService = carsService;
        }

        internal OrdersConfirmationVM CreateReceipt(int orderID, OrdersRentVM viewModel)
        {
            return new OrdersConfirmationVM
            {
                Id = orderID,
                KilometerBeforeRental = ordersRepository.GetKilometreBeforeRental(viewModel.CarId),
                Personnumber = viewModel.personNumber,
                RentalDate = viewModel.RentalDate
            };
        }
        internal int AddOrderToDB(OrdersRentVM viewModel)
        {     
            Orders order = new Orders()
            {
                CarId = viewModel.CarId,
                RentalDate = viewModel.RentalDate,
                ReturnDate = viewModel.ReturnDate,
                KilometerBeforeRental = ordersRepository.GetKilometreBeforeRental(viewModel.CarId),
                CustomerId = customersService.GetCustomersIdFromPersonNumber(viewModel.personNumber)
            };

            ordersRepository.Add(order);
            eventsService.CreateNewOrderEvent(order);
            return order.Id;
        }
        internal OrdersReceiptVM ReturnOrder(OrdersReturnVM viewModel)
        {
            Orders order = ordersRepository.GetOrderById(viewModel.OrderNumber);
            if (order.ReturnDate < viewModel.Date)
                order.ReturnDate = viewModel.Date;
            order.CarReturned = true;
            order.KilometerAfterRental = viewModel.Kilometre;


            int membershipLevel = customersService.GetMembershipLevelByID(order.CustomerId);
            var carType = carsService.GetCarTypeByID(order.CarId);
            var KMDriven = (order.KilometerAfterRental - order.KilometerBeforeRental);

            order.Cost = CalcRentalPrice(carType, order.KilometerAfterRental, order.KilometerBeforeRental, order.ReturnDate, order.RentalDate, membershipLevel);

            ordersRepository.Update(order);
            eventsService.CreateReturnOrderEvent(order);
            customersService.UpdateMembershipLevel(order.Customer, KMDriven);
            carsService.UpdateCarKMByID(order.CarId, order.KilometerAfterRental);

            return new OrdersReceiptVM
            {
                CarReturned = true,
                ReturnDate = viewModel.Date,
                KilometerAfterRental = viewModel.Kilometre,
                Id = viewModel.OrderNumber,
                KilometerBeforeRental = order.KilometerBeforeRental,
                RentalDate = order.RentalDate,
                rentalPrice = order.Cost
            };
        }

        internal AvailableCarsData[] CheckCarsAvailabilityDuringPeriod(RentPeriodData dataModel)
        {

            var cars = carsService.CheckCarsAvailabilityDuringPeriod(dataModel);
            return cars
                 .Select(c => new AvailableCarsData
                 {
                     CarType = c.CarType,
                     Id = c.Id,
                     Registrationnumber = c.Registrationnumber
                 }).ToArray();
        }

        private double CalcRentalPrice(string carType, int kilometerAfterRental, int kilometerBeforeRental, DateTime returnDate, DateTime rentalDate, int membershipLevel)
        {
            double kmPrice = 11.5;
            double multi = 1.5;

            double baseDayRental = 200.0;
            if (membershipLevel > 0)
                baseDayRental /= 2;

            double numberOfDays = (returnDate.Date - rentalDate.Date).TotalDays;
            if (numberOfDays == 3 && membershipLevel > 1)
                numberOfDays = 2;
            if (numberOfDays >= 4 && membershipLevel > 1)
                numberOfDays -= 2;

            double numberOfKm = (kilometerAfterRental - kilometerBeforeRental);
            if (membershipLevel > 2)
            {
                if (numberOfKm <= 20)
                {
                    numberOfKm = 1;
                    kmPrice = 1.0;
                    multi = 1.0;
                }
                if (numberOfKm > 20)
                    numberOfKm -= 20;
            }


            switch (carType)
            {
                case "Small car":
                    return baseDayRental * numberOfDays;
                case "Van":
                    return baseDayRental * numberOfDays * 1.2 * kmPrice * numberOfKm;
                case "Minibus":
                    return baseDayRental * numberOfDays * 1.7 + (kmPrice * numberOfKm * multi);
                default:
                    return 0.0;
            }
        }
        internal int GetKmByID(int orderNumber)
        {
            return ordersRepository.GetKmByID(orderNumber);
        }
    }
}
