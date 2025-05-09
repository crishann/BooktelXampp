using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewBooktel.Data;
using NewBooktel.Models;
using NewBooktel.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;

namespace NewBooktel.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly IEmailSender _emailSender;

        public AuthController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //_emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        #region Registration Actions

        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string FirstName, string LastName, string ContactNumber, string Email, string Password)
        {
            Console.WriteLine("📌 Registering User...");

            FirstName = CapitalizeFirstLetter(FirstName);
            LastName = CapitalizeFirstLetter(LastName);

            //if (await _context.Users.AnyAsync(u => u.Email == Email))
            //{
            //    Console.WriteLine("❌ Email already registered.");
            //    ViewBag.ErrorMessage = "This email is already registered.";
            //    return View("~/Views/Home/Register.cshtml");
            //}

            string hashedPassword = HashPassword(Password);

            var newUser = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                ContactNumber = ContactNumber,
                Email = Email,
                Password = hashedPassword,
                Role = "Guest",
                IsEmailConfirmed = true // ✅ Set to true since you're skipping email confirmation
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // ⛔ Email confirmation disabled
            // var token = Guid.NewGuid().ToString();
            // var confirmationLink = Url.Action("ConfirmEmail", "Auth",
            //     new { email = newUser.Email, token = token }, Request.Scheme);
            // await _emailSender.SendEmailAsync(newUser.Email, "Confirm Your Email",
            //     $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

            Console.WriteLine("✅ User registered successfully (without email confirmation)");
            return RedirectToAction("Login", "Home");
        }


        #endregion

        #region Login Actions

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string? Email, string? Password, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ViewBag.ErrorMessage = "Please enter your email and password.";
                return View("~/Views/Home/Login.cshtml");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            //if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password) || !user.IsEmailConfirmed)
            //{
            //    ViewBag.ErrorMessage = "Invalid login attempt.";
            //    return View("~/Views/Home/Login.cshtml");
            //}

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role?.Trim().ToLower() ?? ""),
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

            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("UserEmail", user.Email);

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
                return RedirectToAction("Index", "Housekeeping");
            }
            else if (user.Role?.Trim().ToLower() == "frontdesk")
            {
                return RedirectToAction("Index", "FrontDesk");
            }
            else
            {
                return RedirectToAction("Reservation", "UserDash");
            }
        }

        #endregion

        #region Logout Action

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");

            Console.WriteLine("✅ User logged out. Session cleared!");
            return RedirectToAction("Login", "Home");
        }

        #endregion

        //#region Email Confirmation

        //[HttpGet]
        //public async Task<IActionResult> ConfirmEmail(string email, string token)
        //{
        //    Console.WriteLine($"📌 Confirming email for {email}");

        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        //    if (user == null)
        //    {
        //        Console.WriteLine("❌ User not found.");
        //        return NotFound("User not found.");
        //    }

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        Console.WriteLine("❌ Invalid confirmation token.");
        //        return BadRequest("Invalid confirmation request.");
        //    }

        //    user.IsEmailConfirmed = true;
        //    await _context.SaveChangesAsync();

        //    Console.WriteLine("✅ Email confirmed successfully!");
        //    return View("EmailConfirmed");
        //}

        //#endregion

        #region Helper Methods

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        #endregion

        #region Other Actions (Potentially Belong Elsewhere)

        public IActionResult HouseKeeping()
        {
            return View("~/Views/HouseKeeping/HouseKeeping.cshtml");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View("~/Views/UserDash/Profile.cshtml");
        }

        #endregion
    }
}