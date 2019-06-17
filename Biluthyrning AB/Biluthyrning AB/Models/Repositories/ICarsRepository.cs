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
        IEnumerable<Cars> ListOfCarsForRemoval();
        IEnumerable<Cars> CheckCarsAvailabilityDuringPeriod(RentPeriodData dataModel);
        bool CheckIfRegistrationNumberAlreadyExists(string regNr);
        Cars Update(Cars carChanges);
        IEnumerable<Cars> GetCarsDetails(int id);
        IEnumerable<Cars> GetAllCars();
        string GetTypeByID(int id);
        Cars GetCarByID(int id);
        void RetireCar(CarRetire cr);
        void ListForCleaning(CarCleaning cc);
        void ListForService(CarService cs);
        void UpdateCleaning(CarCleaning cc);
        CarCleaning GetCarCleaningByID(int id);
        IEnumerable<CarCleaning> GetFullCleaningList();
        IEnumerable<CarService> GetFullServiceList();
        CarService GetCarServiceById(int serviceId);
        void UpdateCarService(CarService cs);
    }
}
