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
        public CarsService(BiluthyrningContext context, EventsService eventService, ICarsRepository carRepository)
        {
            this.context = context;
            this.carRepository = carRepository;
            this.eventsService = eventService;
        }
        readonly BiluthyrningContext context;
        readonly EventsService eventsService;
        private readonly ICarsRepository carRepository;

        //Klara
        internal bool AddNewCar(CarsAddVM viewModel)
        {
            Cars car = new Cars();
            if (!carRepository.CheckIfRegistrationNumberAlreadyExists(viewModel.Registrationnumber))
            {
                car.CarType = viewModel.CarType;
                car.Kilometer = viewModel.Kilometer;
                car.AvailableForRent = true;
                car.Registrationnumber = viewModel.Registrationnumber;

                if (carRepository.Add(car).Id > 0)
                {
                    eventsService.CreateAddedCarEvent(car);
                    return true;
                }
                else
                    return false;                
            }
            else
                return false;
        }
        internal void RemoveCars(CarsRemoveVM[] viewModel)
        {
            foreach (var item in viewModel)
            {
                if (item.Remove)
                {
                    CarRetire car = new CarRetire
                    {
                        CarId = item.CarId,
                        FlaggedForRetiringDate = DateTime.Now,
                        Retired = true,
                        RetiredDate = DateTime.Now
                    };

                    if (carRepository.Delete(car).Id > 0)
                        eventsService.CreateRemovedCarEvent(car);                                        
                }
            }
        }
        internal CarsRemoveVM[] ListOfAllCarsRemove()
        {
            return carRepository.ListOfCarsForRemoval();
        }
        internal CarsDetailsVM GetCarById(int id)
        {
            return carRepository.GetCarsDetails(id);
           
        }
        internal CarsListOfAllVM[] CheckCarsAvailabilityDuringPeriod(RentPeriodData dataModel)
        {
            return carRepository.CheckCarsAvailabilityDuringPeriod(dataModel);    
        }
        internal string GetCarTypeByID(int id)
        {
            return carRepository.GetTypeByID(id);
        }
        private void ListCarForCleaningByID(int id)
        {
            CarCleaning cc = new CarCleaning
            {
                CarId = id,
                FlaggedForCleaningDate = DateTime.Now,
                CleaningDone = false
            };
            carRepository.ListForCleaning(cc);
        }
        private void ListCarForServiceByID(int id)
        {
            CarService cs = new CarService
            {
                CarId = id,
                FlaggedForServiceDate = DateTime.Now,
                ServiceDone = false
            };
            carRepository.ListForService(cs);
        }
        private void RetireCarByID(int id)
        {
            CarRetire cr = new CarRetire
            {
                CarId = id,
                FlaggedForRetiringDate = DateTime.Now,
                Retired = false
            };
            carRepository.RetireCar(cr);
            eventsService.CreateRetireCarEvent(cr);
        }
        internal void UpdateCleaningToDone(int id)
        {
            CarCleaning cc = carRepository.GetCarCleaningByID(id);
            cc.CleaningDone = true;
            cc.CleaningDoneDate = DateTime.Now;
            carRepository.UpdateCleaning(cc);
            eventsService.CreateCleaningCarEvent(cc);           
        }
        
        
        // 

        // Ta bort


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

        internal void UpdateAvailability(int carID)
        {
            carRepository.UpdateAvailability(carID, false);       
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
               


        internal void UpdateCarKMByID(int id, int kilometreAfterRental)
        {
            Cars car = carRepository.GetCarByID(id);
            car.Kilometer = kilometreAfterRental;
            carRepository.Update(car);

            if (car.Kilometer >= 2000)
                RetireCarByID(car.Id);
            else
            {
                ListCarForCleaningByID(car.Id);
                if (car.Orders.Count() % 3 == 0)
                    ListCarForServiceByID(car.Id);
            }

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
