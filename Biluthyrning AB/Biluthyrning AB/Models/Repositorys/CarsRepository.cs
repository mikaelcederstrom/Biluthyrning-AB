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
            throw new NotImplementedException();
        }



        public Cars Update(Cars carChanges)
        {
            context.Cars.Update(carChanges);
            context.SaveChanges();
            return carChanges;
        }

        public void UpdateAvailability(int carId, bool newAvailability)
        {
            Cars car = context.Cars
                .Where(c => c.Id == carId)
                .SingleOrDefault();

            context.Update(car);
            context.SaveChanges();
        }

        public void UpdateCleaning(int cleaningId, bool newCleaningStatus)
        {
            CarCleaning cc = context.CarCleaning
              .Where(c => c.Id == cleaningId)
              .Select(c => new CarCleaning
              {
                  Id = c.Id,
                  CleaningDone = newCleaningStatus,
                  CleaningDoneDate = DateTime.Now,
                  CarId = c.CarId,
                  FlaggedForCleaningDate = c.FlaggedForCleaningDate
              }).SingleOrDefault();
            context.Update(cc);
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
    }
}
