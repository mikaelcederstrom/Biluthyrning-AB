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

        [Route("~/customers/index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [Route("~/customers/add")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Route("~/customers/add")]
        [HttpPost]
        public IActionResult Add(CustomersAddVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            try
            {
            service.AddCustomerToDB(viewModel);
            }
            catch (Exception)
            {
                return View(viewModel);
            }
            return RedirectToAction("Home", "Cars");

        }

        [Route("~/listOfCustomers")]
        [HttpGet]
        public IActionResult ListOfAll()
        {
            return View(service.GetAllCustomersFromDB());

        }

        [Route("~/customers/details/{id}")]
        [HttpGet]
        public IActionResult Details(int id)
        {            
            return View(service.GetCustomerDetailsFromDB(id));

        }
    }
}