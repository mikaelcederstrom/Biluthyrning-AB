using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarsRemoveVM
    {
        public int CarId { get; set; }
        public string CarType { get; set; }
        public string Registrationnumber { get; set; }
        public int Kilometer { get; set; }
        public bool Remove { get; set; }
    }
}
