using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models.Data;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;

namespace Biluthyrning_AB.Models
{
    public class CarsRepository : ICarsRepository
    {
        private readonly BiluthyrningContext context;

        public CarsRepository(BiluthyrningContext context)
        {
            this.context = context;
        }

        public Cars Add(Cars car)
        {
            context.Cars.Add(car);
            context.SaveChanges();
            return car;
        }
        public CarRetire Delete(CarRetire carToDelete)
        {
            context.CarRetire.Add(carToDelete);
            context.SaveChanges();
            return carToDelete;
        }
        public IEnumerable<Cars> ListOfCarsForRemoval()
        {
            return context.Cars
               .Where(k => k.CarRetire.Count == 0)
               .Select(k => new Cars
               {
                   Id = k.Id,
                   CarType = k.CarType,
                   Kilometer = k.Kilometer,
                   Registrationnumber = k.Registrationnumber
               });
        }
        public IEnumerable<Cars> GetCarsDetails(int id)
        {
            IEnumerable<Cars> car = context.Cars
                .Where(c => c.Id == id)
                .Select(c => new Cars
                {
                    AvailableForRent = c.AvailableForRent,
                    CarType = c.CarType,
                    Kilometer = c.Kilometer,
                    Registrationnumber = c.Registrationnumber,
                    Events = c.Events.Select
                            (e => new Events
                            {
                                EventType = e.EventType,
                                BookingId = e.BookingId,
                                Customer = e.Customer,
                                CustomerId = e.CustomerId,
                                Date = e.Date
                            }).OrderByDescending(e => e.Date).ToList()
                });
            return car;
        }
        public IEnumerable<Cars> CheckCarsAvailabilityDuringPeriod(RentPeriodData dataModel)
        {
            return context.Cars
                     .Where(c => (c.CarCleaning.All(k => k.CleaningDone == true) || c.CarCleaning.Count == 0))
                     .Where(c => (!c.CarRetire.Any() || c.CarRetire.Count == 0))
                     .Where(c => (c.CarService.All(s => !s.ServiceDone == false) || c.CarService.Count == 0))
                     .Where(c => (c.Orders.All(o => (!((dataModel.RentalDate >= o.RentalDate && dataModel.RentalDate <= o.ReturnDate) ||
                                  (dataModel.ReturnDate >= o.RentalDate && dataModel.ReturnDate <= o.ReturnDate)) || o.CarReturned == true || c.Orders.Count == 0))))
           .Select(c => new Cars
           {
               AvailableForRent = c.AvailableForRent,
               CarType = c.CarType,
               Id = c.Id,
               Kilometer = c.Kilometer,
               Registrationnumber = c.Registrationnumber,
               Orders = c.Orders
                .Select(o => new Orders
                {
                    Customer = o.Customer
                }).ToList()
           })
           .OrderBy(c => c.AvailableForRent)
           .ThenBy(c => c.CarType);
        }
        public bool CheckIfRegistrationNumberAlreadyExists(string regNr)
        {
            return context.Cars
                    .Where(c => c.Registrationnumber == regNr)
                    .Any();
        }
        public IEnumerable<Cars> GetAllCars()
        {
            return context.Cars
                .Select(c => new Cars
                {
                    CarType = c.CarType,
                    Id = c.Id,
                    Kilometer = c.Kilometer,
                    Registrationnumber = c.Registrationnumber,
                    Orders = c.Orders
                        .Where(o => o.CarReturned == true)
                        .Select(o => new Orders
                        {
                            CarReturned = o.CarReturned
                        }).ToList(),
                    CarCleaning = c.CarCleaning
                    .Where(k => !k.CleaningDone == true)
                    .ToList(),
                    CarService = c.CarService
                    .Where(s => !s.ServiceDone == true)
                    .ToList(),
                    CarRetire = c.CarRetire
                    .ToList()
                });
        }
        public Cars Update(Cars carChanges)
        {
            context.Cars.Update(carChanges);
            context.SaveChanges();
            return carChanges;
        }
        public string GetTypeByID(int id)
        {
            return context.Cars
             .Where(c => c.Id == id)
             .Select(c => c.CarType)
             .FirstOrDefault();
        }
        public Cars GetCarByID(int id)
        {
            return context.Cars
                .Where(c => c.Id == id)
                .SingleOrDefault();
        }
        public void RetireCar(CarRetire cr)
        {
            context.CarRetire.Add(cr);
            context.SaveChanges();
        }
        public void ListForCleaning(CarCleaning cc)
        {
            context.CarCleaning.Add(cc);
            context.SaveChanges();
        }
        public void ListForService(CarService cs)
        {
            context.CarService.Add(cs);
            context.SaveChanges();
        }
        public void UpdateCleaning(CarCleaning cc)
        {
            context.CarCleaning.Update(cc);
            context.SaveChanges();
        }
        public CarCleaning GetCarCleaningByID(int id)
        {
            return context.CarCleaning
                .Where(c => c.Id == id)
                .SingleOrDefault();
        }
        public IEnumerable<CarCleaning> GetFullCleaningList()
        {
            return context.CarCleaning
                .Select(s => new CarCleaning
                {
                    CarId = s.CarId,
                    CleaningDone = s.CleaningDone,
                    CleaningDoneDate = s.CleaningDoneDate,
                    FlaggedForCleaningDate = s.FlaggedForCleaningDate,
                    Id = s.Id
                });
        }
        public IEnumerable<CarService> GetFullServiceList()
        {
            return context.CarService
                .Select(s => new CarService
                {
                    CarId = s.CarId,
                    FlaggedForServiceDate = s.FlaggedForServiceDate,
                    Id = s.Id,
                    ServiceDone = s.ServiceDone,
                    ServiceDoneDate = s.ServiceDoneDate,
                    Car = s.Car
                });
        }
        public CarService GetCarServiceById(int serviceId)
        {
            return context.CarService
             .Where(s => s.Id == serviceId)
             .SingleOrDefault();
        }
        public void UpdateCarService(CarService cs)
        {
            context.CarService.Update(cs);
            context.SaveChanges();
        }
    }
}
