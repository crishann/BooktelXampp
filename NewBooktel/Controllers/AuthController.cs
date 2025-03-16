using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewBooktel.Data;
using NewBooktel.Models;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net; // ✅ Import BCrypt for password hashing

namespace NewBooktel.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml");
        }

        // ✅ GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml");
        }

        // ✅ GET: User Profile Page
        [HttpGet]
        public IActionResult Profile()
        {
            return View("~/Views/UserDash/Profile.cshtml");
        }

        // ✅ POST: Register User (Secure)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string FirstName, string LastName, string ContactNumber, string Email, string Password)
        {
            Console.WriteLine("📌 Registering User...");

            // ✅ Check if the email already exists
            if (await _context.Users.AnyAsync(u => u.Email == Email))
            {
                Console.WriteLine("❌ Email already registered.");
                ViewBag.ErrorMessage = "This email is already registered.";
                return View("~/Views/Home/Register.cshtml");
            }

            // ✅ Hash the password before storing
            string hashedPassword = HashPassword(Password);

            var newUser = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                ContactNumber = ContactNumber,
                Email = Email,
                Password = hashedPassword, // ✅ Store hashed password
                Role = "Guest"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            Console.WriteLine("✅ User Registered Successfully!");
            return RedirectToAction("Login", "Home"); // ✅ Redirect to login page
        }

        // ✅ POST: Login User (Secure)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            Console.WriteLine($"📌 Attempting login for {Email}");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null)
            {
                Console.WriteLine("❌ Invalid login: User not found.");
                //ViewBag.ErrorMessage = "Invalid email or password.";
                return View("~/Views/Home/Login.cshtml");
            }

            // ✅ Check if the stored password is a valid bcrypt hash
            if (!user.Password.StartsWith("$2a$") && !user.Password.StartsWith("$2b$"))
            {
                Console.WriteLine("⚠️ Warning: Password is stored in plaintext. Hashing it now.");

                // ✅ Hash the password and update the database
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = hashedPassword;
                await _context.SaveChangesAsync();
            }

            // ✅ Now safely verify the password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(Password, user.Password);
            if (!isPasswordValid)
            {
                Console.WriteLine("❌ Invalid login: Incorrect password.");
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("~/Views/Home/Login.cshtml");
            }

            // ✅ Store user session
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);

            Console.WriteLine($"✅ Login successful! Welcome, {user.FirstName} ({user.Role})");

            return user.Role == "Admin"
                ? RedirectToAction("AdminIndex", "Admin")
                : RedirectToAction("Reservation", "UserDash");
        }

        // ✅ GET: Logout User
        [HttpGet]
        public IActionResult Logout()
        {
            // ✅ Clear specific session keys
            HttpContext.Session.Remove("UserFirstName");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("UserRole");

            // ✅ Clear all session data
            HttpContext.Session.Clear();

            // ✅ Ensure session cookie is deleted
            Response.Cookies.Delete(".AspNetCore.Session");

            Console.WriteLine("✅ User logged out. Session cleared!");
            return RedirectToAction("Login", "Home");
        }

        // ✅ Helper: Hash password using BCrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
