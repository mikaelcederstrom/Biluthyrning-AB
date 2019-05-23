using System;
using System.Collections.Generic;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class Customers
    {
        public Customers()
        {
            Events = new HashSet<Events>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonNumber { get; set; }

        public virtual ICollection<Events> Events { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
