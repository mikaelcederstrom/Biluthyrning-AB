using System;
using System.Collections.Generic;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class CarService
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime FlaggedForServiceDate { get; set; }
        public DateTime? ServiceDoneDate { get; set; }
        public bool? ServiceDone { get; set; }

        public virtual Cars Car { get; set; }
    }
}
