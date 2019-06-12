using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class OrdersConfirmationVM
    {
        public int Id { get; set; }
        public string Personnumber { get; set; }
        public int KilometerBeforeRental { get; set; }
        public DateTime RentalDate { get; set; }

    }
}
