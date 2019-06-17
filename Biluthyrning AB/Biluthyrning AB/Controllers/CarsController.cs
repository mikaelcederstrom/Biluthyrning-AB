using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models;
using Biluthyrning_AB.Models.Data;
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

        [Route("~/cars/add")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Route("~/cars/add")]
        [HttpPost]
        public IActionResult Add(CarsAddVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            if (!service.AddNewCar(viewModel))
                return View(viewModel);
            
            return RedirectToAction("ListOfAll", "Cars");
        }

        [Route("~/cars/remove")]
        [HttpGet]
        public IActionResult Remove()
        {
            return View(service.ListOfAllCarsRemove());
        }

        [Route("~/cars/remove")]
        [HttpPost]
        public IActionResult Remove(CarsRemoveVM[] viewModel)
        {
            service.RemoveCars(viewModel);
            return RedirectToAction("Remove", "Cars");
        }

        [Route("~/cars/details/{id}")]
        [HttpGet]
        public IActionResult Details(int id)
        {
            return View(service.GetCarById(id));
        }        

        [HttpGet]
        [Route("~/listOfAllCars")]
        public IActionResult ListOfAll()
        {
            return View(service.GetAllCarsFromDB());
        }

        [HttpGet]
        [Route("~/service")]
        public IActionResult Service()
        {
            return View(service.GetServiceListFromDB());
        }

        [HttpPost]
        [Route("~/service/")]
        public IActionResult Service(int id)
        {
            service.UpdateServiceToDone(id);
            return RedirectToAction("Service", "Cars");
        }

        [HttpPost]
        public IActionResult ServiceFromCarList(int id)
        {
            service.UpdateServiceToDone(id);
            return RedirectToAction("ListOfAll", "Cars");
        }

        [HttpGet]
        [Route("~/cleaning")]
        public IActionResult Cleaning()
        {
            return View(service.GetCleaningListFromDB());
        }

        [HttpPost]
        [Route("~/Cleaning/")]
        public IActionResult Cleaning(int id)
        {
            service.UpdateCleaningToDone(id);
            return RedirectToAction("Cleaning", "Cars");
        }

        [HttpPost]
        public IActionResult CleaningFromCarList(int id)
        {
            service.UpdateCleaningToDone(id);
            return RedirectToAction("ListOfAll", "Cars");
        }

        [HttpGet]
        [Route("~/maintenance")]
        public IActionResult Maintenance()
        {
            return View();
        }
    }
}