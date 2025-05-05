using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//[Authorize(Roles = "housekeeping")]
public class HousekeepingController : Controller
{
    // GET: Housekeeping Dashboard
    [Authorize(Roles = "Housekeeping")]
    public IActionResult Housekeeping()
    {
        return View("~/Views/HouseKeeping/HouseKeeping.cshtml");
    }

    // Other actions
    public IActionResult AssignedRooms()
    {
        return View();
    }

    public IActionResult CompletedTasks()
    {
        return View();
    }
}
