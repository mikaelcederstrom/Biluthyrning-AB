using Biluthyrning_AB.Models.Data;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models
{
    public class CarsService
    {

        public CarsService(BiluthyrningContext context)
        {
            this.context = context;
        }
        readonly BiluthyrningContext context;

        internal CarsRentVM DropDownListForCarType()
        {
            Cars[] x = GetAllAvailableCarsFromDB();
            var viewModel = new CarsRentVM();


            viewModel.ListOfCarTypes = new SelectListItem[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                viewModel.ListOfCarTypes[i] = new SelectListItem
                {
                    Value = $"{x[i].Id.ToString()}",
                    Text = $"{x[i].CarType}, {x[i].Registrationnumber}, {x[i].Kilometer} km"
                };
            }
            return viewModel;
        }
        internal CarsListOfAllVM[] CheckCarsAvailabilityDuringPeriod()
        {
            return context.Cars
                .Where(c => (c.CarCleaning.All(k => k.CleaningDone == true) || c.CarCleaning.Count == 0) && (!c.CarRetire.Any() || c.CarRetire.Count == 0) && (c.CarService.All(s => !s.ServiceDone == false) || c.CarService.Count == 0))
                .Select(c => new CarsListOfAllVM
                {

                    AvailableForRent = c.AvailableForRent,
                    CarType = c.CarType,
                    Id = c.Id,
                    Kilometer = c.Kilometer,
                    Registrationnumber = c.Registrationnumber,
                    customer = c.Orders
                     .Where(o => o.CarId == c.Id)
                    .Select(o => new Customers
                    {
                        FirstName = o.Customer.FirstName,
                        LastName = o.Customer.LastName,
                        PersonNumber = o.Customer.PersonNumber,
                        Id = o.CustomerId
                    }).FirstOrDefault()
                })
                .OrderBy(c => c.AvailableForRent)
                .ThenBy(c => c.CarType)
                .ToArray();
        }
        //internal CarsListOfAllVM[] CheckCarsAvailabilityDuringPeriod(RentPeriodData viewModel)
        //{
        //    // 
        //    CarsListOfAllVM[] x = context.Orders
        //        .Where(o => !(o.Car.Orders.Any()) || (!(o.RentalDate.Ticks > viewModel.RentalDate.Ticks) && !(o.ReturnDate.Ticks < viewModel.ReturnDate.Ticks) && o.CarReturned == (false)))
        //        .Select(o => new CarsListOfAllVM
        //        {
        //            AvailableForRent = o.Car.AvailableForRent,
        //            CarType = o.Car.CarType,
        //            Kilometer = o.Car.Kilometer,
        //            Id = o.Car.Id,
        //            Registrationnumber = o.Car.Registrationnumber

        //        }).ToArray();

        //    //CarsListOfAllVM[] y = context.Cars
        //    //    .Where(c => !c.Orders.Any()) || (context.Orders.Where(o => !(o.RentalDate.Ticks > viewModel.RentalDate.Ticks) && !(o.ReturnDate.Ticks < viewModel.ReturnDate.Ticks) && o.CarReturned == (false))
        //    //    .Select(c => new CarsListOfAllVM{
        //    //        AvailableForRent = c.)


        //    return x;
        //}

        internal CarsReceiptVM CreateReceipt(int orderID, CarsRentVM viewModel)
        {
            return new CarsReceiptVM
            {
                Id = orderID,
                KilometerBeforeRental = context.Cars
                    //.Where(c => c.Id == viewModel.SelectedCarTypeValue)
                    .Where(c => c.Id == viewModel.CarId)
                    .Select(c => c.Kilometer)
                    .FirstOrDefault(),
                Personnumber = viewModel.personNumber,
                RentalDate = viewModel.RentalDate
            };
        }

        internal int GetKmFromOrderID(int orderNumber)
        {
            return context.Cars
                .Where(c => c.Orders.Any(o => o.Id == orderNumber)).Select(c => c.Kilometer).SingleOrDefault();
        }

        internal int AddOrderToDB(CarsRentVM viewModel)
        {
            Orders order = new Orders()
            {
                //CarId = viewModel.SelectedCarTypeValue,
                CarId = viewModel.CarId,
                RentalDate = viewModel.RentalDate,
                ReturnDate = viewModel.ReturnDate,
                KilometerBeforeRental = context.Cars
                 .Where(o => o.Id == viewModel.CarId)
                 //.Where(o => o.Id == viewModel.SelectedCarTypeValue)
                 .Select(o => o.Kilometer)
                 .FirstOrDefault(),
                CustomerId = context.Customers
                 .Where(p => p.PersonNumber == viewModel.personNumber)
                 .Select(p => p.Id)
                 .FirstOrDefault()
            };
            context.Orders.Add(order);
            context.SaveChanges();
            //UpdateAvailability(viewModel.SelectedCarTypeValue);
            UpdateAvailability(viewModel.CarId);

            return order.Id;
        }

        internal CarsListOfAllVM[] GetAllCarsFromDB()
        {
            return context.Cars
                .Select(c => new CarsListOfAllVM
                {
                    AvailableForRent = c.AvailableForRent,
                    CarType = c.CarType,
                    Id = c.Id,
                    Kilometer = c.Kilometer,
                    Registrationnumber = c.Registrationnumber,
                    customer = c.Orders
                     .Where(o => o.CarId == c.Id)
                    .Select(o => new Customers
                    {
                        FirstName = o.Customer.FirstName,
                        LastName = o.Customer.LastName,
                        PersonNumber = o.Customer.PersonNumber,
                        Id = o.CustomerId
                    }).FirstOrDefault()
                })
                .OrderBy(c => c.AvailableForRent)
                .ThenBy(c => c.CarType)
                .ToArray();
        }

        internal void UpdateCleaningToDone(int id)
        {
            CarCleaning cc = context.CarCleaning
                .Where(c => c.Id == id)
                .Select(c => new CarCleaning
                {
                    Id = c.Id,
                    CleaningDone = true,
                    CleaningDoneDate = DateTime.Now,
                    CarId = c.CarId,
                    FlaggedForCleaningDate = c.FlaggedForCleaningDate
                }).SingleOrDefault();
            context.Update(cc);
            context.SaveChanges();
        }

        internal CarCleaningVM[] GetCleaningListFromDB()
        {
            return context.CarCleaning
                .Select(s => new CarCleaningVM
                {
                    CarId = s.CarId,
                    CleaningDone = s.CleaningDone,
                    CleaningDoneDate = s.CleaningDoneDate,
                    CleaningId = s.Id,
                    FlaggedForCleaningDate = s.FlaggedForCleaningDate
                }).ToArray();
        }

        internal CarServiceVM[] GetServiceListFromDB()
        {
            return context.CarService
                .Select(s => new CarServiceVM
                {
                    CarId = s.CarId,
                    FlaggedForServiceDate = s.FlaggedForServiceDate,
                    Id = s.Id,
                    ServiceDone = s.ServiceDone,
                    ServiceDoneDate = s.ServiceDoneDate,
                    CarType = context.Cars
                        .Where(c => c.Id == s.CarId)
                        .Select(c => c.CarType).FirstOrDefault(),
                    Kilometer = context.Cars
                        .Where(c => c.Id == s.CarId)
                        .Select(c => c.Kilometer).FirstOrDefault(),
                    Registrationnumber = context.Cars
                        .Where(c => c.Id == s.CarId)
                        .Select(c => c.Registrationnumber).FirstOrDefault(),
                }).ToArray();
        }

        internal void UpdateServiceToDone(int serviceId)
        {
            CarService cs = context.CarService
          .Where(s => s.Id == serviceId)
          .Select(c => new CarService
          {
              Id = c.Id,
              CarId = c.CarId,
              FlaggedForServiceDate = c.FlaggedForServiceDate,
              ServiceDone = true,
              ServiceDoneDate = DateTime.Now

          }).SingleOrDefault();
            context.Update(cs);
            context.SaveChanges();
        }

        private void UpdateAvailability(int carID)
        {
            Cars car = context.Cars
           .Where(c => c.Id == carID)
           .Select(c => new Cars
           {
               Kilometer = c.Kilometer,
               CarType = c.CarType,
               Id = c.Id,
               Registrationnumber = c.Registrationnumber,
               AvailableForRent = false

           }).SingleOrDefault();
            context.Update(car);
            context.SaveChanges();
        }

        internal CarsConfReturnVM ReturnOrderInDB(CarsReturnVM viewModel)
        {
            Orders order = GetOrderByID(viewModel.OrderNumber);
            if (order.ReturnDate < viewModel.Date)
            {
                order.ReturnDate = viewModel.Date;
            }
            order.CarReturned = true;
            order.KilometerAfterRental = viewModel.Kilometer;
            context.Orders.Update(order);
            context.SaveChanges();

            UpdateCarKMByOrderID(viewModel.OrderNumber);

            return new CarsConfReturnVM
            {
                CarReturned = true,
                ReturnDate = viewModel.Date,
                KilometerAfterRental = viewModel.Kilometer,
                Id = viewModel.OrderNumber,

                KilometerBeforeRental = context.Orders
                     .Where(o => o.Id == viewModel.OrderNumber)
                     .Select(o => o.KilometerBeforeRental)
                     .FirstOrDefault(),
                RentalDate = context.Orders
                     .Where(o => o.Id == viewModel.OrderNumber)
                     .Select(o => o.RentalDate)
                     .FirstOrDefault(),
                rentalPrice = CalcRentalPrice(context.Orders.Select(o => o.CarId).FirstOrDefault(), viewModel.Kilometer, context.Orders
                     .Where(o => o.Id == viewModel.OrderNumber)
                     .Select(o => o.KilometerBeforeRental)
                     .FirstOrDefault(), viewModel.Date,
                     context.Orders
                     .Where(o => o.Id == viewModel.OrderNumber)
                     .Select(o => o.RentalDate)
                     .FirstOrDefault())
            };
        }

        private double CalcRentalPrice(int carID, int kilometerAfterRental, int kilometerBeforeRental, DateTime returnDate, DateTime rentalDate)
        {
            const double kmPrice = 11.5;
            const double baseDayRental = 200.0;
            string x = GetCarTypeByID(carID);
            double numberOfDays = (returnDate.Date - rentalDate.Date).TotalDays;
            double numberOfKm = (kilometerAfterRental - kilometerBeforeRental);


            switch (x)
            {
                case "Small car":
                    return baseDayRental * numberOfDays;
                case "Van":
                    return baseDayRental * numberOfDays * 1.2 * kmPrice * numberOfKm;
                case "Minibus":
                    return baseDayRental * numberOfDays * 1.7 + (kmPrice * numberOfKm * 1.5);
                default:
                    return 0.0;
            }
        }

        private string GetCarTypeByID(int carID)
        {

            return context.Cars
                .Where(c => c.Id == carID)
                .Select(c => c.CarType)
                .FirstOrDefault();
        }


        private void UpdateCarKMByOrderID(int orderNumber)
        {
            Cars car = context.Cars
                .Where(c => c.Orders.Any(x => x.Id == orderNumber))
                .Select(c => new Cars
                {
                    Kilometer = context.Orders.Where(o => o.Id == orderNumber).Select(o => o.KilometerAfterRental).SingleOrDefault(),
                    CarType = c.CarType,
                    Id = c.Id,
                    Registrationnumber = c.Registrationnumber,
                    AvailableForRent = true,
                    //Cleaning = true,
                    //NumberOfRentals = c.NumberOfRentals + 1,
                }).SingleOrDefault();


            if (car.Kilometer >= 2000)
                RetireCarByID(car.Id);
            else
            {
                ListCarForCleaningByID(car.Id);
                if (context.Orders.Where(o => o.CarId == car.Id).Count() % 3 == 0)
                    ListCarForServiceByID(car.Id);
            }

            context.Update(car);
            context.SaveChanges();
        }

        private void ListCarForCleaningByID(int id)
        {
            CarCleaning cc = new CarCleaning
            {
                CarId = id,
                FlaggedForCleaningDate = DateTime.Now,
                CleaningDone = false
            };
            context.Add(cc);
            context.SaveChanges();
        }

        private void ListCarForServiceByID(int id)
        {
            CarService cs = new CarService
            {
                CarId = id,
                FlaggedForServiceDate = DateTime.Now,
                ServiceDone = false
            };
            context.Add(cs);
            context.SaveChanges();
        }

        private void RetireCarByID(int id)
        {
            CarRetire cr = new CarRetire
            {
                CarId = id,
                FlaggedForRetiringDate = DateTime.Now,
                Retired = false
            };
            context.Add(cr);
            context.SaveChanges();
        }

        internal Orders GetOrderByID(int id)
        {
            return context.Orders
                .Where(o => o.Id == id)
                .Select(o => new Orders
                {
                    CarId = o.CarId,
                    Id = o.Id,
                    KilometerBeforeRental = o.KilometerBeforeRental,
                    CustomerId = o.CustomerId,
                    RentalDate = o.RentalDate,
                    ReturnDate = o.ReturnDate

                }).SingleOrDefault();
        }

        private Cars[] GetAllCarTypesFromDB()
        {
            return context.Cars
              .OrderBy(p => p.CarType)
              .Select(p => new Cars
              {
                  Id = p.Id,
                  CarType = p.CarType,
                  Kilometer = p.Kilometer,
                  Registrationnumber = p.Registrationnumber

              })
              .ToArray();
        }

        private Cars[] GetAllAvailableCarsFromDB()
        {
            return context.Cars
         .Where(p => p.AvailableForRent == true && !p.CarCleaning.Any(o => o.CleaningDone == false) && !p.CarRetire.Any(x => x.CarId == p.Id))
         .Select(p => new Cars
         {
             Id = p.Id,
             CarType = p.CarType,
             Kilometer = p.Kilometer,
             Registrationnumber = p.Registrationnumber

         })
         .OrderBy(p => p.CarType)
         .ToArray();
        }
    }
}
