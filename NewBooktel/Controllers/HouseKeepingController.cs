using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HousekeepingController : Controller
{
    // GET: Housekeeping Dashboard
    [Authorize(Roles = "housekeeping")]
    public IActionResult Index()
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