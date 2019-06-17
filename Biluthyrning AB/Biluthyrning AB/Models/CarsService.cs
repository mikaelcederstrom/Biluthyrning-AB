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
            var cars = carRepository.ListOfCarsForRemoval().ToArray();

            return cars
                .Select(c => new CarsRemoveVM
                {
                    CarId = c.Id,
                    CarType = c.CarType,
                    Kilometer = c.Kilometer,
                    Registrationnumber = c.Registrationnumber,
                    Remove = false
                }).ToArray(); ;
        }



        internal CarsDetailsVM GetCarById(int id)
        {
            var car = carRepository.GetCarsDetails(id).FirstOrDefault();
           
            CarsDetailsVM cd = new CarsDetailsVM
            {
                AvailableForRent = car.AvailableForRent,
                CarType = car.CarType,
                Kilometer = car.Kilometer,
                Registrationnumber = car.Registrationnumber,
                Events = new CarsDetailsVMEvents[car.Events.Count]
            };

            int j = 0;
            foreach (var item in car.Events)
            {
                cd.Events[j] = new CarsDetailsVMEvents
                {
                    BookingId = item.BookingId,
                    CustomerId = item.CustomerId,
                    Date = item.Date,
                    EventType = eventsService.ConvertEventTypeToString(item.EventType)
                };
                if (item.Customer != null)
                {
                    cd.Events[j].CustomerFirstName = item.Customer.FirstName;
                    cd.Events[j].CustomerLastName = item.Customer.LastName;
                }
                j++;
            }

            return cd;

        }
        internal Cars[] CheckCarsAvailabilityDuringPeriod(RentPeriodData dataModel)
        {
            return carRepository.CheckCarsAvailabilityDuringPeriod(dataModel).ToArray();
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
            var cars = carRepository.GetAllCars().ToArray();

            CarsListOfAllVM[] cl = cars
                .Select(c => new CarsListOfAllVM
                {
                    AvailableForRent = c.Orders.Select(o => o.CarReturned || c.Orders == null).Any(),
                    CarType = c.CarType,
                    CleaningId = c.CarCleaning.Select(s => s.Id).SingleOrDefault(),
                    Kilometer = c.Kilometer,
                    Registrationnumber = c.Registrationnumber,
                    RetireId = c.CarRetire.Select(r => r.Id).SingleOrDefault(),
                    ServiceId = c.CarService.Select(s => s.Id).SingleOrDefault(),
                    Id = c.Id
                })
                .OrderBy(c => c.RetireId)
                .ThenBy(c => c.CarType)
            .ToArray();
            return cl;
        }
        internal CarCleaningVM[] GetCleaningListFromDB()
        {
            CarCleaning[] cc = carRepository.GetFullCleaningList().ToArray();

            return cc
                .Select(c => new CarCleaningVM
                {
                    CarId = c.CarId,
                    CleaningId = c.Id,
                    CleaningDone = c.CleaningDone,
                    CleaningDoneDate = c.CleaningDoneDate,
                    FlaggedForCleaningDate = c.FlaggedForCleaningDate
                }).ToArray();
        }
        internal CarServiceVM[] GetServiceListFromDB()
        {
            CarService[] cs = carRepository.GetFullServiceList().ToArray();

            return cs
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
        internal void UpdateCarKMByID(int id, int kilometreAfterRental)
        {
            Cars car = carRepository.GetCarByID(id);
            car.Kilometer = kilometreAfterRental;
            carRepository.Update(car);

            if (car.Kilometer >= 2000)
            {
                RetireCarByID(car.Id);
                return;
            }
            ListCarForCleaningByID(car.Id);
            if (car.Orders.Count() % 3 == 0)
                ListCarForServiceByID(car.Id);

        }

    }
}
