﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models;
using Biluthyrning_AB.Models.Data;
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
            return RedirectToAction("Confirmation", "Orders", x);
        }

        [HttpGet]
        [Route("~/confirmation")]
        public IActionResult Confirmation(OrdersConfirmationVM viewModel)
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
                return Content("Bilen kan inte lämnas tillbaka med en mätarställning som är lägre än vid hyrning");
            if (!ModelState.IsValid)
                return View(viewModel);

            OrdersReceiptVM or = ordersService.ReturnOrder(viewModel);
            return RedirectToAction("receipt", "Orders", or);
        }

        [HttpGet]
        [Route("~/receipt")]
        public IActionResult Receipt(OrdersReceiptVM viewModel)
        {
            return View(viewModel);
        }

        [HttpPost]
        [Route("~/AvailableCars")]
        public IActionResult AvailableCars([FromBody]RentPeriodData dataModel)
        {
            return AvailableCars(ordersService.CheckCarsAvailabilityDuringPeriod(dataModel));
        }

        [HttpGet]
        [Route("~/AvailableCars")]
        public IActionResult AvailableCars(AvailableCarsData[] x)
        {
            return PartialView("_AvailableCars", x);
        }
    }
}