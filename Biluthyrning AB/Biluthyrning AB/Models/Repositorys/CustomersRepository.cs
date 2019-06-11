using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;

namespace Biluthyrning_AB.Models
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly BiluthyrningContext context;

        public CustomersRepository(BiluthyrningContext context)
        {
            this.context = context;
        }

        public Customers Add(Customers customer)
        {           
            context.Customers.Add(customer);
            context.SaveChanges();
            return customer;
        }

        public Customers Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomersListOfAllVM> GetAllCustomers()
        {
            return context.Customers
                      .Select(c => new CustomersListOfAllVM
                      {
                          FirstName = c.FirstName,
                          LastName = c.LastName,
                          PersonNumber = c.PersonNumber,
                          ID = c.Id,
                          ActiveOrder = c.Orders.Where(o => o.CarReturned == false).Any()
                      }).ToArray();
        }

        public CustomersDetailsVM GetCustomerDetails(int id)
        {
            return context.Customers
                .Where(c => c.Id == id)
                .Select(c => new CustomersDetailsVM
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Id = c.Id,
                    PersonNumber = c.PersonNumber,
                    MembershipLevel = c.MembershipLevel,
                    Orders = c.Orders
                       .Select(o => new CustomersDetailsOrders
                       {
                           CarReturned = o.CarReturned,
                           Id = o.Id,
                           KilometerAfterRental = o.KilometerAfterRental,
                           KilometerBeforeRental = o.KilometerBeforeRental,
                           RentalDate = o.RentalDate,
                           ReturnDate = o.ReturnDate
                       }).ToArray()
                }).FirstOrDefault();
        }

        public int GetIdFromPersonNumber(string personNumber)
        {
            return context.Customers
            .Where(p => p.PersonNumber == personNumber)
            .Select(p => p.Id)
            .FirstOrDefault();
        }

        public int GetMembershipLevelByID(int id)
        {
            return context.Customers
                .Where(o => o.Id == id)
                .Select(o => o.MembershipLevel)
                .FirstOrDefault();
        }

        public Customers Update(Customers customerChanges)
        {
            throw new NotImplementedException();
        }
    }
}
