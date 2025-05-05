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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;

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
        public IActionResult HouseKeeping()
        {
            return View("~/Views/HouseKeeping/HouseKeeping.cshtml");
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Email, string Password, string returnUrl = null)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password) || !user.IsEmailConfirmed)
            {
                ViewBag.ErrorMessage = "Invalid login attempt.";
                return View("~/Views/Home/Login.cshtml");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Or some unique user identifier
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.Trim().ToLower() ?? ""), // Add the user's role as a claim (handle potential null)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                // You can set various authentication properties here, like RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Remove setting role in session, as it's now in the claims
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("UserEmail", user.Email);
            // HttpContext.Session.SetString("UserRole", user.Role); // Remove this line

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (user.Role?.Trim().ToLower() == "admin")
            {
                return RedirectToAction("AdminIndex", "Admin");
            }
            else if (user.Role?.Trim().ToLower() == "housekeeping")
            {
                return RedirectToAction("Index", "Housekeeping"); // Redirect to the correct Housekeeping action
            }
            else
            {
                return RedirectToAction("Reservation", "UserDash");
            }
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