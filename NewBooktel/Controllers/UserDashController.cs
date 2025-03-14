using Microsoft.AspNetCore.Mvc;

public class UserDashController : Controller
{
    public IActionResult Reservation()
    {
        return View("~/Views/UserDash/Reservation.cshtml"); // Use the correct absolute path
    }

}
