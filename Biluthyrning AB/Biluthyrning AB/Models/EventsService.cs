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
            return eventsRepository.GetAllEvents();
        }

        internal void CreateAddedCarEvent(Cars car)
        {
            Events x = new Events()
            {
                EventType = "Bil tillagd",
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
                EventType = "Användare skapad",
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
                EventType = "Bil borttagen",
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
                EventType = "Bil bokad",
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
                 EventType = "Bil återlämnad",
                 CarId = order.CarId,
                 CustomerId = order.CustomerId,
                 BookingId = order.Id,
                 Date = DateTime.Now
             };
            eventsRepository.SaveEvent(x);
        }

        internal void CreateRetireCarEvent(CarRetire cr)
        {
            Events x = new Events()
            {
                EventType = "Bil borttagen",
                CarId = cr.CarId,
                CustomerId = null,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);           
        }

        internal void CreateCleaningCarEvent(CarCleaning cc)
        {
            Events x = new Events()
            {
                EventType = "Bil tvättad",
                CarId = cc.CarId,
                CustomerId = null,
                BookingId = null,
                Date = DateTime.Now
            };
            eventsRepository.SaveEvent(x);
        }
    }
}
