using Biluthyrning_AB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models.Repositorys
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly BiluthyrningContext context;

        public OrdersRepository(BiluthyrningContext context)
        {
            this.context = context;
        }

        public Orders Add(Orders order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
            return order;
        }
        public int GetKilometreBeforeRental(int carId)
        {
           return context.Cars
                    .Where(c => c.Id == carId)
                    .Select(c => c.Kilometer)
                    .FirstOrDefault();
        }
        public int GetKmByID(int orderNumber)
        {
            return context.Cars
                .Where(c => c.Orders.Any(o => o.Id == orderNumber))
                .Select(c => c.Kilometer).SingleOrDefault();
        }
        public Orders GetOrderById(int orderNumber)
        { 
            return context.Orders
                .Where(o => o.Id == orderNumber)
                .Select(o => new Orders
                {
                    CarId = o.CarId,
                    CarReturned = o.CarReturned,
                    Cost = o.Cost,
                    Customer = o.Customer,
                    Id = o.Id,
                    KilometerAfterRental = o.KilometerAfterRental,
                    KilometerBeforeRental = o.KilometerBeforeRental,
                    RentalDate = o.RentalDate,
                    ReturnDate = o.ReturnDate,
                    CustomerId = o.CustomerId
                })
                .SingleOrDefault();
        }
        public void Update(Orders order)
        {
            context.Orders.Update(order);
            context.SaveChanges();
        }
    }
}
