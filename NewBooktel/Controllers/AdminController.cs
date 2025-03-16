using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    // ✅ Restrict Access to Admins Only
    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("UserRole") == "Admin";
    }

    public IActionResult AdminIndex()
    {
        ViewBag.RoomTypes = 3;
        ViewBag.NewBookings = 11;
        ViewBag.ConfirmedBookings = 0;
        ViewBag.SpecialOffers = 0;

        // Sample data for latest bookings table
        ViewBag.LatestBookings = new List<object>
        {
            new { Code = "X880R267", RoomType = "Beach Double Room", CheckIn = DateTime.Parse("06/28/2018"), CheckOut = DateTime.Parse("06/30/2018"), Total = 300, FirstName = "John", LastName = "Smith", Email = "test@test.com", Phone = "212-324-5422" },
            new { Code = "N970N835", RoomType = "Beach Double Room", CheckIn = DateTime.Parse("06/28/2018"), CheckOut = DateTime.Parse("06/30/2018"), Total = 300, FirstName = "John", LastName = "Smith", Email = "test@test.com", Phone = "212-324-5422" }
        };

        if (!IsAdmin()) return RedirectToAction("Index", "Home");
        return View();
    }

    public IActionResult BookingReq()
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");
        return View();
    }

    public IActionResult Checkin()
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");
        return View();
    }

    public IActionResult Checkout()
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");
        return View();
    }

    public IActionResult Housekeeping()
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");
        return View();
    }

    public IActionResult Payment()
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");
        return View();
    }

    public IActionResult Rooms()
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");
        return View();
    }
}
