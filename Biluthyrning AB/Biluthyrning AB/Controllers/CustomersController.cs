using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models;
using Biluthyrning_AB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Biluthyrning_AB.Controllers
{
    public class CustomersController : Controller
    {


        public CustomersController(CustomersService service)
        {
            this.service = service;
        }
        readonly CustomersService service;

        [Route("~/add")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Route("~/add")]
        [HttpPost]
        public IActionResult Add(CustomersAddVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            service.AddCustomerToDB(viewModel);
            return RedirectToAction("Home", "Cars");

        }

        [Route("~/listOfCustomers")]
        [HttpGet]
        public IActionResult ListOfAll()
        {
            return View(service.GetAllCustomersFromDB());

        }
    }
}