using System;
using System.Collections.Generic;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class Orders
    {
        public Orders()
        {
            Events = new HashSet<Events>();
        }

        public int Id { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public int KilometerBeforeRental { get; set; }
        public DateTime RentalDate { get; set; }
        public int KilometerAfterRental { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool CarReturned { get; set; }

        public virtual Cars Car { get; set; }
        public virtual Customers Customer { get; set; }
        public virtual ICollection<Events> Events { get; set; }
    }
}
