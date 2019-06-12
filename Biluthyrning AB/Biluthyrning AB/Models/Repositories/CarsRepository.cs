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
        public CarsRemoveVM[] ListOfCarsForRemoval()
        {
            return context.Cars
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
        }
        public CarsDetailsVM GetCarsDetails(int id)
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
        public CarsListOfAllVM[] CheckCarsAvailabilityDuringPeriod(RentPeriodData dataModel)
        {
            return context.Cars
           .Where(c => (c.CarCleaning.All(k => k.CleaningDone == true) || c.CarCleaning.Count == 0) &&
                                               (!c.CarRetire.Any() || c.CarRetire.Count == 0) &&
                                               (c.CarService.All(s => !s.ServiceDone == false) || c.CarService.Count == 0)
                                               && (c.Orders.All(o => (!((dataModel.RentalDate >= o.RentalDate && dataModel.RentalDate <= o.ReturnDate) || (dataModel.ReturnDate >= o.RentalDate && dataModel.ReturnDate <= o.ReturnDate)) || o.CarReturned == true || c.Orders.Count == 0))))
           .Select(c => new CarsListOfAllVM
           {
               AvailableForRent = true,
               CarType = c.CarType,
               Id = c.Id,
               Kilometer = c.Kilometer,
               Registrationnumber = c.Registrationnumber,
               customer = c.Orders
               .Select(o => o.Customer)
               .FirstOrDefault()
           })
           .OrderBy(c => c.AvailableForRent)
           .ThenBy(c => c.CarType)
           .ToArray();
        }
        public bool CheckIfRegistrationNumberAlreadyExists(string regNr)
        {
            return context.Cars
                    .Where(c => c.Registrationnumber == regNr)
                    .Any();
        }               
        public IEnumerable<CarsListOfAllVM> GetAllCars()
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
                    //.Where(o => o.CarId == c.Id)
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
        public CarCleaningVM[] GetFullCleaningList()
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
        public CarServiceVM[] GetFullServiceList()
        {
            return context.CarService
                .Select(s => new CarServiceVM
                {
                    CarId = s.CarId,
                    FlaggedForServiceDate = s.FlaggedForServiceDate,
                    Id = s.Id,
                    ServiceDone = s.ServiceDone,
                    ServiceDoneDate = s.ServiceDoneDate,
                    CarType = s.Car.CarType,
                    Kilometer = s.Car.Kilometer,
                    Registrationnumber = s.Car.Registrationnumber
                }).ToArray();
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
