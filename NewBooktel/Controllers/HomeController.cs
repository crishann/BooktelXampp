using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewBooktel.Models;

namespace NewBooktel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reservation()
        {
            ViewData["ActivePage"] = "Reservation";
            return View("UserDash/Reservation"); // ✅ Specify the correct folder
        }


        public IActionResult Contact()
        {
            ViewData["ActivePage"] = "Contact";
            return View();
        }

        public IActionResult Register()
        {
            ViewData["ActivePage"] = "Register";
            return View();
        }

        public IActionResult Login()
        {
            ViewData["ActivePage"] = "Login";
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View("~/Views/UserDash/Profile.cshtml");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
