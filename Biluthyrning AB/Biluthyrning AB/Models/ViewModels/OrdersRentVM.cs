using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class OrdersRentVM
    {
        public string TempMessage { get; set; }

        [Display(Name = "Personnummer")]
        [Required(ErrorMessage = "Skriv ditt personnummer")]
        [StringLength(11, ErrorMessage = "Personnumret ska vara 11 tecken", MinimumLength = 11)]
        public string personNumber { get; set; }
        public string Registrationnumber { get; set; }
        public int Kilometer { get; set; }


        public int CarId { get; set; }


        [Display(Name = "Välj bil")]
        public SelectListItem[] ListOfCarTypes { get; set; }

        public int SelectedCarTypeValue { get; set; }

        [Display(Name = "Datum för hyrning")]
        [Range(typeof(DateTime), "1/1/2019", "1/1/2021", ErrorMessage = "Måste välja datum mellan 2019-01-01 & 2021-01-01")]
        public DateTime RentalDate { get; set; }

        [Display(Name = "Datum för återlämning")]
        [Range(typeof(DateTime), "1/1/2019", "1/1/2021", ErrorMessage = "Måste välja datum mellan 2019-01-01 & 2021-01-01")]
        public DateTime ReturnDate { get; set; }

    }
}
