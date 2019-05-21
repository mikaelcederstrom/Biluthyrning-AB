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
        [HttpPost]
        [Route("~/AvailableCars")]
        public IActionResult AvailableCars([FromBody]RentPeriodData viewModel)
        {
            return Json(new { success = true });

            //return RedirectToAction(nameof(AvailableCars));

        }
        [HttpGet]
        [Route("~/AvailableCars")]
        public IActionResult AvailableCars()
        {

            //CarsListOfAllVM[] x = service.CheckCarsAvailabilityDuringPeriod(viewModel);
            CarsListOfAllVM[] x = service.CheckCarsAvailabilityDuringPeriod();
            return PartialView("_AvailableCars", x);

        }

        [HttpGet]
        [Route("~/rent/")]
        public IActionResult Rent()
        {
            return View(service.DropDownListForCarType());
        }

        [HttpPost]
        [Route("~/rent")]
        public IActionResult Rent(CarsRentVM viewModel)
        {
            //CarsRentVM z = service.DropDownListForCarType();
            //for (int i = 0; i < z.ListOfCarTypes.Length; i++)
            //{
            //    viewModel.ListOfCarTypes[i] = z.ListOfCarTypes[i];
            //}
            
            if (!ModelState.IsValid)
                return View(viewModel);
            
            CarsReceiptVM x = service.CreateReceipt(service.AddOrderToDB(viewModel), viewModel);
            return RedirectToAction("Receipt", "Cars", x);
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
            viewModel.KilometerBeforeRental = service.GetKmFromOrderID(viewModel.OrderNumber);                        


            if (viewModel.Kilometer < viewModel.KilometerBeforeRental)
                return Content("Bilen kan inte lämnas tillbaka med en mätarställning som är lägre än vid hyrning"); // Return partialview, med texten 
            if (!ModelState.IsValid || viewModel.Kilometer < viewModel.KilometerBeforeRental)
                return View(viewModel);

            CarsConfReturnVM cr =  service.ReturnOrderInDB(viewModel);
            return RedirectToAction("ConfReturn", "Cars", cr);
        }

        [HttpGet]
        [Route("~/receipt")]
        public IActionResult Receipt(CarsReceiptVM viewModel)
        {
            return View(viewModel);
        }

        [HttpGet]
        [Route("~/confReturn")]
        public IActionResult ConfReturn(CarsConfReturnVM viewModel)
        {
            return View(viewModel);
        }

        [HttpGet]
        [Route("~/listOfAllCars")]
        public IActionResult ListOfAll()
        {
            CarsListOfAllVM[] x = service.GetAllCarsFromDB();
            return View(x);

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

        [HttpGet]
        [Route("~/maintenance")]
        public IActionResult Maintenance()
        {

            return View();
        }
    }
}