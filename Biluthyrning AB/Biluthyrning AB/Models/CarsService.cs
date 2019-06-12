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
        public CarsService(EventsService eventService, ICarsRepository carRepository)
        {
            this.carRepository = carRepository;
            this.eventsService = eventService;
        }
        readonly EventsService eventsService;
        private readonly ICarsRepository carRepository;

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
        internal void UpdateServiceToDone(int serviceId)
        {
            CarService cs = carRepository.GetCarServiceById(serviceId);
            cs.ServiceDone = true;
            cs.ServiceDoneDate = DateTime.Now;
            carRepository.UpdateCarService(cs);
            eventsService.CreateServiceCarEvent(cs);
        }

        internal void UpdateAvailability(int carID)
        {
            Cars car = carRepository.GetCarByID(carID);
            car.AvailableForRent = false;
            carRepository.Update(car);
        }
        internal CarsListOfAllVM[] GetAllCarsFromDB()
        {
            return carRepository.GetAllCars().ToArray();
        }
        internal CarCleaningVM[] GetCleaningListFromDB()
        {
            return carRepository.GetFullCleaningList();
        }
        internal CarServiceVM[] GetServiceListFromDB()
        {
            return carRepository.GetFullServiceList();
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

    }
}
