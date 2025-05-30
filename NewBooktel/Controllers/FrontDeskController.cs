﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewBooktel.Controllers
{
    [Authorize(Roles = "frontdesk")]
    public class FrontDeskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult CheckIn()
        {
            return View();
        }
        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult Reservation()
        {
            return View();
        }
        public IActionResult Payment()
        {
            return View();
        }
        public IActionResult Guest()
        {
            return View();
        }

    }
}
