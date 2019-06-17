using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biluthyrning_AB.Models;
using Biluthyrning_AB.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Biluthyrning_AB.Controllers
{
    public class EventsController : Controller
    {
        public EventsController(EventsService service)
        {
            this.service = service;
        }
        readonly EventsService service;

        [Route("event/index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View(service.GetAllEventFromDB());
        }
    }
}