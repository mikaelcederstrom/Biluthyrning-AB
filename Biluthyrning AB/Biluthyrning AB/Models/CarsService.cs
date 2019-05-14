using Biluthyrning_AB.Models.Entities;
using Biluthyrning_AB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models
{
    public class CarsService
    {
        const string MessageKey = "Message";
        public CarsService(BiluthyrningContext context)
        {
            this.context = context;
        }
        readonly BiluthyrningContext context;

        internal CarsRentVM DropDownListForCarType(string message)
        {
            Cars[] x = GetAllCarTypesFromDB();
            var viewModel = new CarsRentVM();

            viewModel.TempMessage = message;
            viewModel.ListOfCarTypes = new SelectListItem[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                viewModel.ListOfCarTypes[i] = new SelectListItem
                {
                    Value = $"{x[i].Id.ToString()}",
                    Text = $"{x[i].CarType}, {x[i].Registrationnumber}, {x[i].Kilometer} km"
                };
            }
            return viewModel;
        }

        internal int AddOrderToDB(CarsRentVM viewModel)
        {
            Orders order = new Orders()
            {
                CarId = viewModel.SelectedCarTypeValue,
                RentalDate = viewModel.Date,
                KilometerBeforeRental = context.Cars
                 .Where(o => o.Id == viewModel.SelectedCarTypeValue)
                 .Select(o => o.Kilometer)
                 .FirstOrDefault(),
                 Personnumber = viewModel.personNumber
            };
            context.Orders.Add(order);
            context.SaveChanges();

            return order.Id;       
        }

        internal void ReturnOrderInDB(CarsReturnVM viewModel)
        {
            Orders order = GetOrderByID(viewModel.OrderNumber);
            order.ReturnDate = viewModel.Date;
            order.CarReturned = true;
            order.KilometerAfterRental = viewModel.Kilometer;
            context.Orders.Update(order);
            context.SaveChanges();

            UpdateCarKMByOrderID(viewModel.OrderNumber);               
                
        }

        private void UpdateCarKMByOrderID(int orderNumber)
        {
            Cars car = context.Cars
                .Where(c => c.Orders.Any(x => x.Id == orderNumber))
                .Select(c => new Cars
                {
                    Kilometer = context.Orders.Where(o => o.Id == orderNumber).Select(o => o.KilometerAfterRental).SingleOrDefault(),
                    CarType = c.CarType,
                    Id = c.Id,
                    Registrationnumber = c.Registrationnumber
                }).SingleOrDefault();
            context.Update(car);
            context.SaveChanges();
        }

        internal Orders GetOrderByID(int id)
        {
            return context.Orders
                .Where(o => o.Id == id)
                .Select(o => new Orders
                {
                    CarId = o.CarId,
                    Id = o.Id,
                    KilometerBeforeRental = o.KilometerBeforeRental,
                    Personnumber = o.Personnumber,
                    RentalDate = o.RentalDate,

                }).SingleOrDefault(); 
        }

        private Cars[] GetAllCarTypesFromDB()
        {
            return context.Cars
              .OrderBy(p => p.CarType)
              .Select(p => new Cars
              {
                  Id = p.Id,
                  CarType = p.CarType,
                  Kilometer = p.Kilometer,
                  Registrationnumber = p.Registrationnumber                    
                  
              })
              .ToArray();
        }
    }
}
