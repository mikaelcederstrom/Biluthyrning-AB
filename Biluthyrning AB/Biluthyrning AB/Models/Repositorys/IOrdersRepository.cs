using Biluthyrning_AB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.Repositorys
{
    public interface IOrdersRepository
    {
        Orders Add(Orders order);
        int GetKilometreBeforeRental(int carId);
        int GetKmByID(int orderNumber);
        Orders GetOrderById(int orderNumber);
        void Update(Orders order);
    }
}
