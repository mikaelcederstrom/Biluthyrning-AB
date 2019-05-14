using System;
using System.Collections.Generic;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class Cars
    {
        public Cars()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string CarType { get; set; }
        public string Registrationnumber { get; set; }
        public int Kilometer { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
