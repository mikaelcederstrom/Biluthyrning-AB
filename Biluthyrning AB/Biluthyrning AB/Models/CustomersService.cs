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
                    PersonNumber = c.PersonNumber,
                    ID = c.Id,
                    ActiveOrder = c.Orders.Where(o => o.CarReturned == false).Any()

                })
                .OrderByDescending(c => c.ActiveOrder).ThenBy(c => c.PersonNumber)
                .ToArray();
        }

        internal CustomersDetailsVM GetCustomerDetailsFromDB(int id)
        {
            return context.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomersDetailsVM
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Id = c.Id,
                    PersonNumber = c.PersonNumber,
                    Orders = context.Orders
                    .Where(o => o.CustomerId == id)
                    .Select(o => new CustomersDetailsOrders
                    {
                         CarReturned = o.CarReturned,
                         Id = o.Id,
                         KilometerAfterRental = o.KilometerAfterRental,
                         KilometerBeforeRental =  o.KilometerBeforeRental,
                         RentalDate = o.RentalDate,
                         ReturnDate = o.ReturnDate
                    }).ToArray()                    
                }).FirstOrDefault();            
        }
    }
}
