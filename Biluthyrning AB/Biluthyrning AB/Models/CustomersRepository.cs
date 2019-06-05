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

        public IEnumerable<Customers> GetAllCustomers()
        {
            throw new NotImplementedException();
        }

        public Customers GetCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public Customers Update(Customers customerChanges)
        {
            throw new NotImplementedException();
        }
    }
}
