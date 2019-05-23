using System;
using System.Collections.Generic;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class Cars
    {
        public Cars()
        {
            CarCleaning = new HashSet<CarCleaning>();
            CarRetire = new HashSet<CarRetire>();
            CarService = new HashSet<CarService>();
            Events = new HashSet<Events>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string CarType { get; set; }
        public string Registrationnumber { get; set; }
        public int Kilometer { get; set; }
        public bool AvailableForRent { get; set; }

        public virtual ICollection<CarCleaning> CarCleaning { get; set; }
        public virtual ICollection<CarRetire> CarRetire { get; set; }
        public virtual ICollection<CarService> CarService { get; set; }
        public virtual ICollection<Events> Events { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
