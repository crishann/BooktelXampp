using Microsoft.AspNetCore.Mvc;
using NewBooktel.Data;
using NewBooktel.Models;
using System;

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
                // 🟡 Retrieve form values
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

                // 🔵 Compute number of nights
                int nights = (checkOutDate - checkInDate).Days;
                if (nights <= 0)
                {
                    ModelState.AddModelError("", "Check-out date must be after check-in date.");
                    return View("~/Views/Booking/BookingForm.cshtml");
                }

                // 🟣 Determine price per night
                decimal pricePerNight = roomType switch
                {
                    "Standard" => 2000,
                    "Deluxe" => 2800,
                    "Suite" => 3500,
                    _ => 0
                };

                decimal totalAmount = pricePerNight * nights;

                // 🟢 Create booking object
                var booking = new Booking
                {
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
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                // 🔴 Save to database
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
