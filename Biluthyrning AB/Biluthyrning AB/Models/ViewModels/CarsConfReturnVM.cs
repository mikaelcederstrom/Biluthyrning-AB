using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarsConfReturnVM
    {
        public double rentalPrice { get; set; }
        public int Id { get; set; }
        public int CarId { get; set; }
        public int KilometerBeforeRental { get; set; }
        public DateTime RentalDate { get; set; }
        public int KilometerAfterRental { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool CarReturned { get; set; }


    }
}
