using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class EventsIndexVM
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public int? CarId { get; set; }
        public string CarRegNr { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public int? BookingId { get; set; }
        public DateTime Date { get; set; }
    }
}
