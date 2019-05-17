using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;

namespace Biluthyrning_AB.Models
{
    public class CustomersService
    {

        public CustomersService(BiluthyrningContext context)
        {
            this.context = context;
        }
        readonly BiluthyrningContext context;
        internal void AddCustomerToDB(CustomersAddVM viewModel)
        {
            Customers x = new Customers
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PersonNumber = viewModel.PersonNumber
            };

            context.Customers.Add(x);
            context.SaveChanges();
        }

        internal CustomersListOfAll[] GetAllCustomersFromDB()
        {
            return context.Customers
                .Select(c => new CustomersListOfAll
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PersonNumber = c.PersonNumber
                })
                .OrderBy(c => c.PersonNumber)
                .ToArray();
        }
    }
}
