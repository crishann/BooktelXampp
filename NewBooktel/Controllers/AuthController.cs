using Microsoft.AspNetCore.Mvc;
using NewBooktel.Data;
using NewBooktel.Models;

namespace NewBooktel.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ Ensures CSRF protection
        public IActionResult Register(User model)
        {
            System.Diagnostics.Debug.WriteLine("📌 Register POST method hit!");

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("❌ ModelState is INVALID!");
                foreach (var error in ModelState.Values)
                {
                    foreach (var subError in error.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"🔴 Validation Error: {subError.ErrorMessage}");
                    }
                }
                return View(model); // Return view with validation errors
            }

            System.Diagnostics.Debug.WriteLine("✅ ModelState is VALID!");
            System.Diagnostics.Debug.WriteLine($"👤 Saving user: {model.FirstName} {model.LastName}");

            // Check if email is already registered
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                System.Diagnostics.Debug.WriteLine("❌ Email already exists!");
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            // Set default role
            model.Role = "Guest";

            // Save user to the database
            _context.Users.Add(model);
            _context.SaveChanges();

            System.Diagnostics.Debug.WriteLine("✅ User saved successfully!");

            return RedirectToAction("Login", "Auth");
        }
    }
}
