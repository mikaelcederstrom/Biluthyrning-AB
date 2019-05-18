using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarCleaningVM
    {
        public int CleaningId { get; set; }
        public int CarId { get; set; }
        public DateTime FlaggedForCleaningDate { get; set; }
        public DateTime? CleaningDoneDate { get; set; }
        public bool? CleaningDone { get; set; }

    }
}
