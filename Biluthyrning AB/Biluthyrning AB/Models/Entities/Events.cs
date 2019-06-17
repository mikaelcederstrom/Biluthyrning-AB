using System;
using System.Collections.Generic;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class Events
    {
        public int Id { get; set; }
        public int EventType { get; set; }
        public int? CarId { get; set; }
        public int? CustomerId { get; set; }
        public int? BookingId { get; set; }
        public DateTime Date { get; set; }

        public virtual Orders Booking { get; set; }
        public virtual Cars Car { get; set; }
        public virtual Customers Customer { get; set; }
    }
}
