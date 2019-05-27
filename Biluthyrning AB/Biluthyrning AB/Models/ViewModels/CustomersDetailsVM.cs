using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CustomersDetailsVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonNumber { get; set; }
        public int MembershipLevel { get; set; }
        public CustomersDetailsOrders[] Orders { get; set; }
    }

    public class CustomersDetailsOrders
    {
        public int Id { get; set; }
        //public int CarId { get; set; }
        //public int CustomerId { get; set; }
        public int KilometerBeforeRental { get; set; }
        public DateTime RentalDate { get; set; }
        public int KilometerAfterRental { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool CarReturned { get; set; }

    }
}
