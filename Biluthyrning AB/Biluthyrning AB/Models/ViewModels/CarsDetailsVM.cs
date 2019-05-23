using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarsDetailsVM
    {
        public string CarType { get; set; }
        public string Registrationnumber { get; set; }
        public int Kilometer { get; set; }
        public bool AvailableForRent { get; set; }
        public CarsDetailsVMEvents[] Events { get; set; }
    }
    public class CarsDetailsVMEvents
    {
        public string EventType { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public int? BookingId { get; set; }
        public DateTime Date { get; set; }
    }
}
