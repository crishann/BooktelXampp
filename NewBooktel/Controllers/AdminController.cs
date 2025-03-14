using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult AdminIndex()
    {
        return View(); // No need for absolute path anymore!
    }

    public IActionResult BookingReq()
    {
        return View();
    }

    public IActionResult Checkin()
    {
        return View();
    }

    public IActionResult Checkout()
    {
        return View();
    }

    public IActionResult Housekeeping()
    {
        return View();
    }

    public IActionResult Payment()
    {
        return View();
    }

    public IActionResult Rooms()
    {
        return View();
    }
}
