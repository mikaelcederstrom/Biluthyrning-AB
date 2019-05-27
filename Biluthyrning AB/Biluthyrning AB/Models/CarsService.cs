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
        internal CarsListOfAllVM[] CheckCarsAvailabilityDuringPeriod(RentPeriodData viewModel)
        {
            CarsListOfAllVM[] x = context.Cars
                .Where(c => (c.CarCleaning.All(k => k.CleaningDone == true) || c.CarCleaning.Count == 0) &&
                                                    (!c.CarRetire.Any() || c.CarRetire.Count == 0) &&
                                                    (c.CarService.All(s => !s.ServiceDone == false) || c.CarService.Count == 0)
                                                    && (c.Orders.All(o => (!((viewModel.RentalDate >= o.RentalDate && viewModel.RentalDate <= o.ReturnDate) || (viewModel.ReturnDate >= o.RentalDate && viewModel.ReturnDate <= o.ReturnDate)) || o.CarReturned == true || c.Orders.Count == 0))))
                .Select(c => new CarsListOfAllVM
                {
                    AvailableForRent = true,
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
            return x;
        }

        internal CarsDetailsVM GetCarById(int id)
        {
            return context.Cars
                .Where(c => c.Id == id)
                .Select(c => new CarsDetailsVM
                {
                    AvailableForRent = c.AvailableForRent,
                    CarType = c.CarType,
                    Kilometer = c.Kilometer,
                    Registrationnumber = c.Registrationnumber,
                    Events = c.Events.Select(e => new CarsDetailsVMEvents
                    {
                        EventType = e.EventType,
                        BookingId = e.BookingId,
                        CustomerFirstName = e.Customer.FirstName,
                        CustomerLastName = e.Customer.LastName,
                        CustomerId = e.CustomerId,
                        Date = e.Date
                    }).OrderByDescending(e => e.Date)
                    .ToArray()

                }).FirstOrDefault();
        }

        internal void RemoveCars(CarsRemoveVM[] viewModel)
        {
            foreach (var item in viewModel)
            {
                if (item.Remove)
                {
                    CarRetire x = new CarRetire
                    {
                        CarId = item.CarId,
                        FlaggedForRetiringDate = DateTime.Now,
                        Retired = true,
                        RetiredDate = DateTime.Now
                    };
                    context.CarRetire.Add(x);
                    Events y = AddEventToDB("Bil borttagen", null, x.CarId, null);
                    context.Events.Add(y);
                }
            }
            context.SaveChanges();
        }

        private Events AddEventToDB(string eventType, int? customerId, int? carId, int? orderId)
        {
            Events x = new Events();
            x.EventType = eventType;
            x.CarId = carId;
            x.CustomerId = customerId;
            x.BookingId = orderId;
            x.Date = DateTime.Now;
            return x;
        }

        internal CarsRemoveVM[] ListOfAllCarsRemove()
        {
            CarsRemoveVM[] x = context.Cars
                .Where(c => c.CarRetire.Count == 0)
                .Select(c => new CarsRemoveVM
                {
                    CarId = c.Id,
                    CarType = c.CarType,
                    Kilometer = c.Kilometer,
                    Registrationnumber = c.Registrationnumber,
                    Remove = false
                })
                .ToArray();

            return x;
        }

        internal bool AddCarToDB(CarsAddVM viewModel)
        {
            Cars x = new Cars
            {
                CarType = viewModel.CarType,
                Kilometer = viewModel.Kilometer,
                AvailableForRent = true
            };
            if (context.Cars.All(c => c.Registrationnumber != viewModel.Registrationnumber))
                x.Registrationnumber = viewModel.Registrationnumber;
            else
                return false;


            context.Cars.Add(x);
            context.SaveChanges();
            Events y = AddEventToDB("Bil tillagd", null, x.Id, null);
            context.Events.Add(y);
            context.SaveChanges();
            return true;
        }

        internal CarsListOfAllVM[] CheckCarsAvailabilityDuringPeriodAAAAA(RentPeriodData viewModel)
        {
            // 
            CarsListOfAllVM[] x = context.Orders
                .Where(o => !(o.Car.Orders.Any()) || (!(o.RentalDate.Ticks > viewModel.RentalDate.Ticks) && !(o.ReturnDate.Ticks < viewModel.ReturnDate.Ticks) && o.CarReturned == (false)))
                .Select(o => new CarsListOfAllVM
                {
                    AvailableForRent = o.Car.AvailableForRent,
                    CarType = o.Car.CarType,
                    Kilometer = o.Car.Kilometer,
                    Id = o.Car.Id,
                    Registrationnumber = o.Car.Registrationnumber

                }).ToArray();

            return x;
        }

        internal CarsReceiptVM CreateReceipt(int orderID, CarsRentVM viewModel)
        {
            return new CarsReceiptVM
            {
                Id = orderID,
                KilometerBeforeRental = context.Cars
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

                CarId = viewModel.CarId,
                RentalDate = viewModel.RentalDate,
                ReturnDate = viewModel.ReturnDate,
                KilometerBeforeRental = context.Cars
                 .Where(o => o.Id == viewModel.CarId)
                 .Select(o => o.Kilometer)
                 .FirstOrDefault(),
                CustomerId = context.Customers
                 .Where(p => p.PersonNumber == viewModel.personNumber)
                 .Select(p => p.Id)
                 .FirstOrDefault()
            };
            context.Orders.Add(order);
            context.SaveChanges();
            Events y = AddEventToDB("Bil bokad", order.CustomerId, order.CarId, order.Id);
            context.Events.Add(y);
            context.SaveChanges();
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
                    }).FirstOrDefault(),
                    CleaningId = c.CarCleaning
                    .Where(k => k.CarId == c.Id && !k.CleaningDone == true)
                    .Select(k => k.Id).FirstOrDefault(),
                    ServiceId = c.CarService
                    .Where(s => s.CarId == c.Id && !s.ServiceDone == true)
                    .Select(s => s.Id).FirstOrDefault(),
                    RetireId = c.CarRetire
                    .Where(r => r.CarId == c.Id)
                    .Select(r => r.Id).FirstOrDefault(),


                })
                .OrderBy(c => c.RetireId)
                .ThenBy(c => c.AvailableForRent)
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
            Events y = AddEventToDB("Bil tvättad", null, cc.CarId, null);
            context.Events.Add(y);
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
            Events y = AddEventToDB("Service av bil", null, cs.CarId, null);
            context.Events.Add(y);
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
                order.ReturnDate = viewModel.Date;
            order.CarReturned = true;
            order.KilometerAfterRental = viewModel.Kilometer;
            var rentalDate = context.Orders.Where(o => o.Id == viewModel.OrderNumber).Select(o => o.RentalDate).FirstOrDefault();
            var membershipLevel = context.Orders.Where(o => o.Id == viewModel.OrderNumber).Select(o => o.Customer.MembershipLevel).FirstOrDefault();
            order.Cost = CalcRentalPrice(context.Orders.Where(o => o.Id == viewModel.OrderNumber).Select(o => o.CarId).FirstOrDefault(), viewModel.Kilometer, viewModel.KilometerBeforeRental, viewModel.Date, rentalDate, membershipLevel);

            context.Orders.Update(order);


            Events y = AddEventToDB("Bil återlämnad", order.CustomerId, order.CarId, order.Id);
            context.Events.Add(y);
            context.SaveChanges();

            UpdateCarKMByOrderID(viewModel.OrderNumber);
            UpdateCustomersKMAndNrOfOrders(viewModel.OrderNumber);
            context.SaveChanges();


            //return context.Orders
            //    .Where(o => o.Id == viewModel.OrderNumber)
            //    .Select(o => new CarsConfReturnVM
            //    {
            //        CarReturned = true,
            //        ReturnDate = viewModel.Date,
            //        KilometerAfterRental = viewModel.Kilometer,
            //        Id = viewModel.OrderNumber,
            //        KilometerBeforeRental = o.KilometerBeforeRental,
            //        RentalDate = o.RentalDate,
            //        rentalPrice = CalcRentalPrice(CARID, viewModel.Kilometer, o.KilometerBeforeRental, viewModel.Date, o.RentalDate)
            //    }).FirstOrDefault();

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
                 .FirstOrDefault(),
                 membershipLevel)
            };
        }

        private void UpdateCustomersKMAndNrOfOrders(int orderNumber)
        {
            Customers x = context.Orders
                .Where(o => o.Id == orderNumber)
                .Select(o => new Customers
                {
                    FirstName = o.Customer.FirstName,
                    LastName = o.Customer.LastName,
                    Id = o.Customer.Id,
                    NumberOfOrders = o.Customer.NumberOfOrders + 1,
                    KilometersDriven = o.Customer.KilometersDriven + (o.KilometerAfterRental - o.KilometerBeforeRental),
                    PersonNumber = o.Customer.PersonNumber,
                    MembershipLevel = o.Customer.MembershipLevel
                }).FirstOrDefault();

            x.MembershipLevel = UpdateMembershipLevel(x.MembershipLevel, x.NumberOfOrders, x.KilometersDriven, x.Id);
            context.Customers.Update(x);
        }

        private int UpdateMembershipLevel(int membershipLevel, int numberOfOrders, double kilometersDriven, int id)
        {
            if (membershipLevel == 0 && numberOfOrders >= 3)
            {
                membershipLevel = 1;
                Events y = AddEventToDB("Medlemskap uppgraderad", id, null, null);
                context.Events.Add(y);
                context.SaveChanges();
            }
            if (membershipLevel == 1 && numberOfOrders >= 5)
            {
                membershipLevel = 2;
                Events y = AddEventToDB("Medlemskap uppgraderad", id, null, null);
                context.Events.Add(y);
                context.SaveChanges();
            }

            if (membershipLevel == 2 && kilometersDriven >= 1000)
            {
                membershipLevel = 3;
                Events y = AddEventToDB("Medlemskap uppgraderad", id, null, null);
                context.Events.Add(y);
                context.SaveChanges();
            }
            return membershipLevel;

        }

        private double CalcRentalPrice(int carID, int kilometerAfterRental, int kilometerBeforeRental, DateTime returnDate, DateTime rentalDate, int membershipLevel)
        {
            double kmPrice = 11.5;
            double multi = 1.5;

            double baseDayRental = 200.0;
            if (membershipLevel > 0)
                baseDayRental /= 2;

            string x = GetCarTypeByID(carID);

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


            switch (x)
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
                    AvailableForRent = true
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
            Events y = AddEventToDB("Bil borttagen", null, cr.CarId, null);
            context.Events.Add(y);
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
