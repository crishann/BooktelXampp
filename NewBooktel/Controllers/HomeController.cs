using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewBooktel.Models;
using NewBooktel.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySqlConnector;

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

        public IActionResult Reserve()
        {
            return View();
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage()
        {
            string name = Request.Form["name"];
            string email = Request.Form["email"];
            string message = Request.Form["message"];

            string connectionString = "server=localhost;database=bookteldb;user=root;password=;";
            string query = "INSERT INTO contactus(name, email, message) VALUES (@name, @email, @message)";

            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@message", message);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            TempData["Message"] = "Message sent successfully!";
            return RedirectToAction("Contact");
        }

    }
}
