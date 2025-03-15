using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NewBooktel.Data;
using NewBooktel.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NewBooktel.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 TEST FUNCTION: Add a user manually to test database connection
        [HttpGet]
        public IActionResult AddTestUser()
        {
            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                ContactNumber = "09123456789",
                Email = "johndoe@example.com",
                Password = "password123", // ❗ Hash this in production!
                Role = "Guest"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Content("✅ User added successfully!");
        }

        // 🔹 GET Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml");
        }

        // 🔹 GET Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml"); // ✅ Explicitly set path
        }

        [HttpGet]
        public IActionResult Profile() { 
            return View("~/Views/UserDash/Profile.cshtml");
        }


        // 🔹 POST Register Function
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string FirstName, string LastName, string ContactNumber, string Email, string Password)
        {
            System.Diagnostics.Debug.WriteLine("📌 Registering User...");

            // ✅ Check if email is already registered
            var existingUser = await _context.Users
                .Where(u => u.Email == Email)
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                System.Diagnostics.Debug.WriteLine("❌ Email already exists!");
                ViewBag.ErrorMessage = "This email is already registered.";
                return View();
            }

            // ✅ Insert new user manually (NO User.cs)
            await _context.Database.ExecuteSqlRawAsync(
                "INSERT INTO Users (FirstName, LastName, ContactNumber, Email, Password, Role) " +
                "VALUES (@FirstName, @LastName, @ContactNumber, @Email, @Password, 'Guest')",
                new MySqlParameter("@FirstName", FirstName),
                new MySqlParameter("@LastName", LastName),
                new MySqlParameter("@ContactNumber", ContactNumber),
                new MySqlParameter("@Email", Email),
                new MySqlParameter("@Password", Password) // ❗ WARNING: Password should be hashed!
            );

            System.Diagnostics.Debug.WriteLine("✅ User Registered Successfully!");

            return RedirectToAction("Login", "Home"); // Redirect to login page after registration
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            System.Diagnostics.Debug.WriteLine("📌 Attempting login for " + Email);

            // ✅ Check if user exists
            var user = await _context.Users
                .Where(u => u.Email == Email && u.Password == Password) // ⚠️ Hash passwords in production!
                .FirstOrDefaultAsync();

            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine("❌ Invalid login!");
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View();
            }

            // ✅ Store user session (to remember login)
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);

            System.Diagnostics.Debug.WriteLine($"✅ Login successful! Welcome, {user.FirstName} ({user.Role})");

            // ✅ Redirect based on user role
            if (user.Role == "Admin")
            {
                return RedirectToAction("AdminIndex", "Admin"); // ✅ Admins go to Admin Dashboard
            }
            else
            {
                return RedirectToAction("Reservation", "UserDash"); // ✅ Guests go to UserDash
            }
        }


        // 🔹 Logout Function
        [HttpGet]
        public IActionResult Logout()
        {
            // ✅ Remove specific session keys
            HttpContext.Session.Remove("UserFirstName");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserRole");

            // ✅ Clear all session data
            HttpContext.Session.Clear();

            // ✅ Ensure session cookie is deleted (fix for some browsers)
            Response.Cookies.Delete(".AspNetCore.Session");

            System.Diagnostics.Debug.WriteLine("✅ User logged out. Session cleared!");

            return RedirectToAction("Login", "Home"); // ✅ Redirect to the correct login page
        }


    }
}
