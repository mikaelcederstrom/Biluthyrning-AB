using Biluthyrning_AB.Models.Data;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models
{
    public interface ICarsRepository
    {
        Cars Add(Cars car);
        CarRetire Delete(CarRetire carToDelete);
        CarsRemoveVM[] ListOfCarsForRemoval();
        CarsListOfAllVM[] CheckCarsAvailabilityDuringPeriod(RentPeriodData dataModel);


        bool CheckIfRegistrationNumberAlreadyExists(string regNr);
        Cars Update(Cars carChanges);




        CarsDetailsVM GetCarsDetails(int id);
        IEnumerable<CarsListOfAllVM> GetAllCars();

        void UpdateAvailability(int carId, bool newAvailability);
        void UpdateCleaning(int cleaningId, bool newCleaningStatus);
        string GetTypeByID(int id);
        Cars GetCarByID(int id);
        void RetireCar(CarRetire cr);
        void ListForCleaning(CarCleaning cc);
        void ListForService(CarService cs);
        void UpdateCleaning(CarCleaning cc);
        CarCleaning GetCarCleaningByID(int id);
    }
}
