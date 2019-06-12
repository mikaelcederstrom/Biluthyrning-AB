using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models;
using Biluthyrning_AB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Biluthyrning_AB.Controllers
{
    public class OrdersController : Controller
    {
        public OrdersController(OrdersService ordersService)
        {
            this.ordersService = ordersService;
        }
        readonly OrdersService ordersService;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("~/rent/")]
        public IActionResult Rent()
        {
            return View();
        }

        [HttpPost]
        [Route("~/rent")]
        public IActionResult Rent(OrdersRentVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            OrdersConfirmationVM x = ordersService.CreateReceipt(ordersService.AddOrderToDB(viewModel), viewModel);
            return RedirectToAction("Receipt", "Orders", x);
        }
        [HttpGet]
        [Route("~/receipt")]
        public IActionResult Receipt(OrdersConfirmationVM viewModel)
        {
            return View(viewModel);
        }

        [HttpGet]
        [Route("~/return")]
        public IActionResult Return()
        {
            return View();
        }

        [HttpPost]
        [Route("~/return")]
        public IActionResult Return(OrdersReturnVM viewModel)
        {
            viewModel.KilometerBeforeRental = ordersService.GetKmByID(viewModel.OrderNumber);

            if (viewModel.Kilometre < viewModel.KilometerBeforeRental)
                return Content("Bilen kan inte lämnas tillbaka med en mätarställning som är lägre än vid hyrning"); // Return partialview, med texten 
            if (!ModelState.IsValid)
                return View(viewModel);

            OrdersReceiptVM or = ordersService.ReturnOrder(viewModel);
            return RedirectToAction("ConfReturn", "Orders", or);
        }

        [HttpGet]
        [Route("~/confReturn")]
        public IActionResult ConfReturn(OrdersReceiptVM viewModel)
        {
            return View(viewModel);
        }
    }
}