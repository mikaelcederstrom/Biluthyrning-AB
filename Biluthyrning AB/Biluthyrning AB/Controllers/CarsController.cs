using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models;
using Biluthyrning_AB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Biluthyrning_AB.Controllers
{
    public class CarsController : Controller
    {
        
        public CarsController(CarsService service)
        {
            this.service = service;            
        }
        readonly CarsService service;
        public IActionResult Index()
        {
            return View();
        }
        [Route("")]
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        [Route("~/rent/")]
        public IActionResult Rent(string tempData)
        {
            return View(service.DropDownListForCarType(tempData));
        }

        [HttpPost]
        [Route("~/rent")]
        public IActionResult Rent(CarsRentVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            string tempData = $"Din bokning är gjord och har ordernummer {service.AddOrderToDB(viewModel)}";
            return RedirectToAction("Rent", "Cars", tempData);
        }

        [HttpGet]
        [Route("~/return")]
        public IActionResult Return()
        {
            return View();
        }

        [HttpPost]
        [Route("~/return")]
        public IActionResult Return(CarsReturnVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            service.ReturnOrderInDB(viewModel);
            return RedirectToAction("Return", "Cars");
        }
    }
}