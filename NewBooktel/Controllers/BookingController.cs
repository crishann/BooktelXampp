using Microsoft.AspNetCore.Mvc;
using NewBooktel.Data;
using NewBooktel.Models;
using System;
using System.Security.Claims;

namespace NewBooktel.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book()
        {
            try
            {
                string userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr))
                {
                    ModelState.AddModelError("", "User is not logged in.");
                    return View("~/Views/Booking/BookingForm.cshtml");
                }
                int userId = int.Parse(userIdStr);

                DateTime checkInDate = DateTime.Parse(Request.Form["CheckInDate"]);
                DateTime checkOutDate = DateTime.Parse(Request.Form["CheckOutDate"]);
                int guest = int.Parse(Request.Form["Guest"]);
                string roomType = Request.Form["RoomType"];
                string fullName = Request.Form["FullName"];
                string email = Request.Form["Email"];
                string phoneNumber = Request.Form["PhoneNumber"];
                string address = Request.Form["Address"];
                string specialRequests = Request.Form["SpecialRequests"];
                string paymentMethod = Request.Form["PaymentMethod"];

                int nights = (checkOutDate - checkInDate).Days;
                if (nights <= 0)
                {
                    ModelState.AddModelError("", "Check-out date must be after check-in date.");
                    return View("~/Views/Booking/BookingForm.cshtml");
                }

                // ✅ Fetch price from the Room table based on RoomType (Name)
                var room = _context.Rooms.FirstOrDefault(r => r.Name == roomType);
                if (room == null)
                {
                    ModelState.AddModelError("", $"Room type '{roomType}' not found.");
                    return View("~/Views/Booking/BookingForm.cshtml");
                }

                decimal totalAmount = room.Price * nights;

                var booking = new Booking
                {
                    UserId = userId,
                    CheckInDate = checkInDate,
                    CheckOutDate = checkOutDate,
                    Guest = guest,
                    RoomType = roomType,
                    FullName = fullName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    SpecialRequests = specialRequests,
                    PaymentMethod = paymentMethod,
                    PaymentStatus = "Pending",
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Booking failed: " + ex.Message);
                ModelState.AddModelError("", "Booking failed. Please try again.");
                return View("~/Views/Booking/BookingForm.cshtml");
            }
        }

        // ✅ Success Page   
        public IActionResult Success()
        {
            return View("~/Views/Booking/Success.cshtml");
        }


    }
}
