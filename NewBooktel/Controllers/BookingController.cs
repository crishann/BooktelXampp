using Microsoft.AspNetCore.Mvc;
using NewBooktel.Models;
using System;

public class BookingController : Controller
{
    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        if (ModelState.IsValid)
        {
            // Ensure both CheckInDate and CheckOutDate have values
            if (booking.CheckInDate.HasValue && booking.CheckOutDate.HasValue)
            {
                // Calculate total amount based on RoomType and dates
                TimeSpan? duration = booking.CheckOutDate - booking.CheckInDate;
                int nights = duration.HasValue ? duration.Value.Days : 0; // Handle potential null TimeSpan

                // Sample pricing logic (you can move this to a service layer)
                decimal pricePerNight = booking.RoomType switch
                {
                    "Standard" => 2000,
                    "Deluxe" => 2800,
                    "Suite" => 3500,
                    _ => 0
                };

                booking.TotalAmount = pricePerNight * nights;
                booking.Status = "Pending";
                booking.CreatedAt = DateTime.Now;

                // Save to DB (if using EF Core, inject DbContext and add here)
                // _context.Bookings.Add(booking);
                // _context.SaveChanges();

                return View("~/Views/UserDash/Success.cshtml");
                // or redirect to invoice
            }
            else
            {
                // Handle the case where CheckInDate or CheckOutDate is null
                ModelState.AddModelError("", "Check-in and check-out dates are required.");
            }
        }

        return View(booking); // if invalid or dates are missing, re-show form with errors
    }

    public IActionResult Success()
    {
        return View(); // show booking success message
    }
}