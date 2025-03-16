using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewBooktel.Data;
using NewBooktel.Models;
using NewBooktel.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net; // ✅ Import BCrypt for password hashing

namespace NewBooktel.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender; // ✅ Properly initialized

        // ✅ Constructor: Initializes both _context and _emailSender
        public AuthController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
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

            // ✅ Capitalize First Name and Last Name before saving
            FirstName = CapitalizeFirstLetter(FirstName);
            LastName = CapitalizeFirstLetter(LastName);

            // ✅ Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == Email))
            {
                Console.WriteLine("❌ Email already registered.");
                ViewBag.ErrorMessage = "This email is already registered.";
                return View("~/Views/Home/Register.cshtml");
            }

            // ✅ Hash password before storing
            string hashedPassword = HashPassword(Password);

            var newUser = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                ContactNumber = ContactNumber,
                Email = Email,
                Password = hashedPassword,
                Role = "Guest",
                IsEmailConfirmed = false
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // ✅ Generate confirmation link
            var token = Guid.NewGuid().ToString();
            var confirmationLink = Url.Action("ConfirmEmail", "Auth",
                new { email = newUser.Email, token = token }, Request.Scheme);

            // ✅ Send confirmation email
            await _emailSender.SendEmailAsync(newUser.Email, "Confirm Your Email",
                $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

            Console.WriteLine("✅ Confirmation email sent!");
            return RedirectToAction("Login", "Home");
        }


        // ✅ POST: Login User (Secure)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("~/Views/Home/Login.cshtml");
            }

            if (!user.IsEmailConfirmed)
            {
                ViewBag.ErrorMessage = "Please verify your email before logging in.";
                return View("~/Views/Home/Login.cshtml");
            }

            // ✅ Validate password
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(Password, user.Password);
            if (!isPasswordValid)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("~/Views/Home/Login.cshtml");
            }

            // ✅ Store session
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", user.Role);

            return RedirectToAction("Reservation", "UserDash");
        }

        // ✅ GET: Logout User
        [HttpGet]
        public IActionResult Logout()
        {
            // ✅ Clear session data
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");

            Console.WriteLine("✅ User logged out. Session cleared!");
            return RedirectToAction("Login", "Home");
        }

        // ✅ Helper: Hash password using BCrypt
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // ✅ GET: Confirm Email
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            Console.WriteLine($"📌 Confirming email for {email}");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                Console.WriteLine("❌ User not found.");
                return NotFound("User not found.");
            }

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("❌ Invalid confirmation token.");
                return BadRequest("Invalid confirmation request.");
            }

            // ✅ Mark email as confirmed
            user.IsEmailConfirmed = true;
            await _context.SaveChangesAsync();

            Console.WriteLine("✅ Email confirmed successfully!");
            return View("EmailConfirmed");
        }
        public string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }
    }
}