using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewBooktel.Models;
using NewBooktel.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NewBooktel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoomService _roomService; // ✅ Inject RoomService

        public HomeController(ILogger<HomeController> logger, RoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetAllRoomsAsync(); // ✅ Fetch rooms from database
            return View(rooms); // ✅ Pass rooms to the View
        }

        public IActionResult Reservation()
        {
            ViewData["ActivePage"] = "Reservation";
            return View("UserDash/Reservation");
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
