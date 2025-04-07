using Microsoft.AspNetCore.Mvc;
using NewBooktel.Models;


public class BookingController : Controller
{
    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        if (ModelState.IsValid)
        {
            // Calculate total amount based on RoomType and dates
            var nights = (booking.CheckOutDate - booking.CheckInDate).Days;

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

        return View(booking); // if invalid, re-show form with errors
    }

    public IActionResult Success()
    {
        return View(); // show booking success message
    }
}
