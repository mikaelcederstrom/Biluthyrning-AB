using System;
using System.Collections.Generic;

namespace Biluthyrning_AB.Models.Entities
{
    public partial class CarRetire
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime FlaggedForRetiringDate { get; set; }
        public DateTime? RetiredDate { get; set; }
        public bool? Retired { get; set; }

        public virtual Cars Car { get; set; }
    }
}
