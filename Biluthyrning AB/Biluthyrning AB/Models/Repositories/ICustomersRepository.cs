using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models
{
    public interface ICustomersRepository
    {
        Customers GetCustomerDetails(int id);
        IEnumerable<Customers> GetAllCustomers();
        Customers Add(Customers customer);
        Customers Update(Customers customerChanges);
        Customers Delete(int id);
        int GetIdFromPersonNumber(string personNumber);
        int GetMembershipLevelByID(int id);
    }
}
