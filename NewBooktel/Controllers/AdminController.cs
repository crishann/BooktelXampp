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
