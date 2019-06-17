using Biluthyrning_AB.Models.Data;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models
{
    public class EventsService
    {
        public EventsService(IEventsRepository eventsRepository)
        {
            this.eventsRepository = eventsRepository;
        }
        private readonly IEventsRepository eventsRepository;

        internal EventsIndexVM[] GetAllEventFromDB()
        {
            Events[] events = eventsRepository.GetAllEvents();
            EventsIndexVM[] ie = new EventsIndexVM[events.Length];

            for (int i = 0; i < ie.Length; i++)
            {
                ie[i] = new EventsIndexVM
                {               
                    Date = events[i].Date,
                    EventType = ConvertEventTypeToString(events[i].EventType),
                    Id = events[i].Id,
                };

                if (events[i].Customer != null)
                {
                    ie[i].CustomerFirstName = events[i].Customer.FirstName;
                    ie[i].CustomerLastName = events[i].Customer.LastName;
                    ie[i].CustomerId = events[i].CustomerId;
                };

                if (events[i].Car != null)
                {
                    ie[i].CarRegNr = events[i].Car.Registrationnumber;                  
                    ie[i].CarId = events[i].CarId;
                }
            }

            return ie;

        }
        internal void CreateAddedCarEvent(Cars car)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.CarAdded,
                CarId = car.Id,
                CustomerId = null,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
        internal void CreateAddedCustomerEvent(Customers customer)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.CustomerAdded,
                CarId = null,
                CustomerId = customer.Id,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
        internal void CreateRemovedCarEvent(CarRetire car)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.CarRemoved,
                CarId = car.CarId,
                CustomerId = null,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
        internal void CreateNewOrderEvent(Orders order)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.CarBooked,
                CarId = order.CarId,
                CustomerId = order.CustomerId,
                BookingId = order.Id,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
        internal void CreateReturnOrderEvent(Orders order)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.CarReturned,
                CarId = order.CarId,
                CustomerId = order.CustomerId,
                BookingId = order.Id,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
        internal void CreateMembershipUpdatedEvent(Customers customer)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.MembershipUpgraded,
                CarId = null,
                CustomerId = customer.Id,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
        internal void CreateRetireCarEvent(CarRetire cr)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.CarRemoved,
                CarId = cr.CarId,
                CustomerId = null,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }

        internal string ConvertEventTypeToString(int eventType)
        {
            switch (eventType)
            {
                case 0: return "Bil bokad";
                case 1: return "Bil återlämnad";
                case 2: return "Bil tvättad";
                case 3: return "Service av bil";
                case 4: return "Bil tillagd";
                case 5: return "Bil borttagen";
                case 6: return "Användare skapad";
                case 7: return "Användare borttagen";
                case 8: return "Medlemskap uppgraderad";
                default:
                    return "Wrong input";

            }
        }

        internal void CreateCleaningCarEvent(CarCleaning cc)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.CarWashed,
                CarId = cc.CarId,
                CustomerId = null,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
        internal void CreateServiceCarEvent(CarService cs)
        {
            Events x = new Events()
            {
                EventType = (int)EventType.CarServiced,
                CarId = cs.CarId,
                CustomerId = null,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
    }
}
