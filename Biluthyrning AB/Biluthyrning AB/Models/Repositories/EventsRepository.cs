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

        public EventsIndexVM[] GetAllEvents()
        {
            return context.Events
                .Select(e => new EventsIndexVM
                {
                    BookingId = e.BookingId,
                    CarId = e.CarId,
                    CarRegNr = e.Car.Registrationnumber,
                    CustomerFirstName = e.Customer.FirstName,
                    CustomerId = e.CustomerId,
                    CustomerLastName = e.Customer.LastName,
                    Date = e.Date,
                    EventType = e.EventType,

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
