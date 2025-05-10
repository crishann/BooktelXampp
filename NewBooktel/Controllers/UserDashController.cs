using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewBooktel.Data;
using NewBooktel.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using NewBooktel.ViewModels;

[Authorize] // ✅ Apply authorization at the controller level - requires login for all actions
public class UserDashController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserDashController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ Booking Form - Handles initial display and pre-filled data
    public IActionResult Index(DateTime? CheckInDate, DateTime? CheckOutDate, int? Guest, string RoomType)
    {
        var bookingModel = new Booking { RoomType = "Default" };

        if (CheckInDate.HasValue)
        {
            bookingModel.CheckInDate = CheckInDate.Value;
        }
        if (CheckOutDate.HasValue)
        {
            bookingModel.CheckOutDate = CheckOutDate.Value;
        }
        if (Guest.HasValue)
        {
            bookingModel.Guest = Guest.Value;
        }
        if (!string.IsNullOrEmpty(RoomType))
        {
            bookingModel.RoomType = RoomType;
        }


        if (CheckInDate.HasValue && CheckOutDate.HasValue && Guest.HasValue && !string.IsNullOrEmpty(RoomType))
        {
            var booking = new Booking
            {
                CheckInDate = CheckInDate.Value,
                CheckOutDate = CheckOutDate.Value,
                Guest = Guest.Value,
                RoomType = RoomType
            };

            ViewBag.BookingDetails = booking; // Pass booking details to view
        }
        // Specify the full path to the BookingForm view
        return View("~/Views/Booking/BookingForm.cshtml", bookingModel);
    }


    // ✅ Reservation Page - Accessible to logged-in users
    public IActionResult Reservation()
    {
        return View("~/Views/UserDash/Reservation.cshtml");
    }

    // ✅ Profile Page - Get User Data
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        string? userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        // Fetch user's bookings
        var bookings = await _context.Bookings
            .Where(b => b.UserId == user.Id)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        // Wrap both in ViewModel
        var viewModel = new UserProfileViewModel
        {
            User = user,
            Bookings = bookings
        };

        return View(viewModel);
    }

    // ✅ Update Profile via AJAX (Recommended)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile([FromForm] string Email, [FromForm] string NewPassword)
    {
        string? userEmail = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(userEmail))
        {
            return Json(new { success = false, message = "Unauthorized access. Please log in." });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return Json(new { success = false, message = "User not found." });
        }

        // Check if new email is already taken
        if (Email != user.Email && await _context.Users.AnyAsync(u => u.Email == Email))
        {
            return Json(new { success = false, message = "This email is already in use." });
        }

        user.Email = Email;

        if (!string.IsNullOrEmpty(NewPassword))
        {
            user.Password = HashPassword(NewPassword);
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        HttpContext.Session.SetString("UserEmail", user.Email);

        return Json(new { success = true, message = "Profile updated successfully!" });
    }

    // ✅ Secure Password Hashing using BCrypt
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}