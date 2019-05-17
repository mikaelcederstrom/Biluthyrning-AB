using Biluthyrning_AB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarsListOfAllVM
    {
        public int Id { get; set; }
        public string CarType { get; set; }
        public string Registrationnumber { get; set; }
        public int Kilometer { get; set; }
        public bool AvailableForRent { get; set; }
        public Customers customer { get; set; }
    }
}
