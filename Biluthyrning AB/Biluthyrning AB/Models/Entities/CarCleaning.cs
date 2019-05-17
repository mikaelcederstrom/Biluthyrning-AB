using System;
using System.Collections.Generic;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class CarCleaning
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime FlaggedForCleaningDate { get; set; }
        public DateTime? CleaningDoneDate { get; set; }
        public bool? CleaningDone { get; set; }

        public virtual Cars Car { get; set; }
    }
}
