using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;

namespace Biluthyrning_AB.Models
{
    public class EventsRepository : IEventsRepository
    {
        private readonly BiluthyrningContext context;

        public EventsRepository(BiluthyrningContext context)
        {
            this.context = context;
        }

        public Events[] GetAllEvents()
        {
            return context.Events
                .Select(e => new Events
                {
                    Id = e.Id,
                    BookingId = e.BookingId,
                    CarId = e.CarId,
                    Car = e.Car,
                    Customer = e.Customer,
                    Date = e.Date,
                    EventType = e.EventType,
                    CustomerId = e.CustomerId
                }).OrderByDescending(e => e.Date)
                  .ToArray();
        }
        public void SaveEvent(Events e)
        {
            context.Events.Add(e);
            context.SaveChanges();
        }
    }
}
