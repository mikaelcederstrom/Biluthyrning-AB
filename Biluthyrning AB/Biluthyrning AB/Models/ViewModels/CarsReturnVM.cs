using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CarsReturnVM
    {

        [Display(Name = "Bokningsnummer")]
        [Required(ErrorMessage = "Skriv in bokningsnumret")]        
        public int OrderNumber { get; set; }

        [Display(Name = "Mätarställning")]
        [Required(ErrorMessage = "Skriv in mätarställningen")]
        public int Kilometer { get; set; }

        [Display(Name = "Tid för återlämning")]
        [Range(typeof(DateTime), "1/1/2019", "1/1/2021", ErrorMessage = "Date is out of Range")]
        public DateTime Date { get; set; }
    }
}
