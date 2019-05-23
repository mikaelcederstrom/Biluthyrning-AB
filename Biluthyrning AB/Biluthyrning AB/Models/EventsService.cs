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
        public EventsService(BiluthyrningContext context)
        {
            this.context = context;
        }
        readonly BiluthyrningContext context;

        internal EventsIndexVM[] GetAllEventFromDB()
        {
            EventsIndexVM[] x = context.Events
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
            return x;
        }
    }
}
