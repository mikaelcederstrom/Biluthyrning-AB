using Biluthyrning_AB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.Data
{
    public class AvailableCarsData
    {
        public int Id { get; set; }
        public string CarType { get; set; }
        public string Registrationnumber { get; set; }
    }
}
