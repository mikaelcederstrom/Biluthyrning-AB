using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarsAddVM
    {
        [Display(Name = "Typ av bil")]
        [Required(ErrorMessage = "Välj typ av bil")]       
        public string CarType { get; set; }
        [Display(Name = "Registreringsnummer för bilen")]
        [Required(ErrorMessage = "Skriv ett registreringsnummer")]
        [StringLength(6, ErrorMessage = "Registreringsnummer måste vara 6 tecken", MinimumLength = 6)]
        public string Registrationnumber { get; set; }
        [Display(Name = "Nuvarande mätarställning i kilometer")]
        [Required(ErrorMessage = "Skriv in en mätarställning i kilometer")]
        public int Kilometer { get; set; }

        //[Display(Name = "Välj typ av bil")]
        //public SelectListItem[] ListOfCarTypes { get; set; }

        //public int SelectedCarTypeValue { get; set; }
    }
}
