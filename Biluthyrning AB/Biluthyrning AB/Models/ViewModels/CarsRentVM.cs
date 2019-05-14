using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarsRentVM
    {
        public string TempMessage { get; set; }

        [Display(Name = "Personnummer")]
        [Required(ErrorMessage = "Skriv ditt personnummer")]
        [StringLength(10, ErrorMessage = "Personnumret ska vara 10 tecken", MinimumLength = 10)]
        public string personNumber { get; set; }
        public string Registrationnumber { get; set; }
        public int Kilometer { get; set; }

               
        [Display(Name = "Välj bil")]
        public SelectListItem[] ListOfCarTypes { get; set; }

        public int SelectedCarTypeValue { get; set; }

        [Range(typeof(DateTime), "1/1/2019", "1/1/2021", ErrorMessage = "Date is out of Range")]
        public DateTime Date { get; set; }

    }
}
