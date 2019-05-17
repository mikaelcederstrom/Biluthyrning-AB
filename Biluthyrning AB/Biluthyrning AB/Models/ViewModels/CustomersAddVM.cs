using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CustomersAddVM
    {
        [Display(Name = "Förnamn")]
        [Required(ErrorMessage = "Förnamn måste fyllas i")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Efternamn måste fyllas i")]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Display(Name = "Personnummer")]
        [Required(ErrorMessage = "Skriv ditt personnummer")]
        [StringLength(11, ErrorMessage = "Personnumret måste skrivas 890129-1111", MinimumLength = 11)]
        public string PersonNumber { get; set; }
    }
}
