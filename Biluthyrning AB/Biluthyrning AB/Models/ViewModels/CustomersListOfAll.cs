using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.ViewModels
{
    public class CustomersListOfAll
    {
        public int ID { get; set; }
        public string FirstName { get; set; }               
        public string LastName { get; set; }
        public string PersonNumber { get; set; }
        public bool ActiveOrder { get; set; }

    }
}
