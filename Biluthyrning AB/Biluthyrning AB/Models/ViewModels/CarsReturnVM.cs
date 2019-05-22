using Biluthyrning_AB.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarsReturnVM
    {

        [Display(Name = "Bokningsnummer")]
        [Required(ErrorMessage = "Skriv in bokningsnumret")]        
        public int OrderNumber { get; set; }

        [Display(Name = "Mätarställning")]
        [Required(ErrorMessage = "Skriv in mätarställningen")]
        [Range(0.0, 3000.0)]
        public int Kilometer { get; set; }
        
        public int KilometerBeforeRental { get; set; }

        [Display(Name = "Tid för återlämning")]
        [Range(typeof(DateTime), "1/1/2019", "1/1/2021", ErrorMessage = "Date is out of Range")]
        public DateTime Date { get; set; }
    }
}
