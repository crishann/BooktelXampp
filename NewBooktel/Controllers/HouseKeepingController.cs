using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewBooktel.Models;
using NewBooktel.Services;
using System.Threading.Tasks;

namespace NewBooktel.Controllers
{
    [Authorize(Roles = "housekeeping")]
    public class HousekeepingController : Controller
    {
        private readonly RoomTaskOperations _service;

        public HousekeepingController(RoomTaskOperations service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var pendingTasks = await _service.GetPendingAsync();
            var completedTasks = await _service.GetCompletedAsync();

            var viewModel = new HousekeepingViewModel
            {
                PendingTasks = pendingTasks,
                CompletedTasks = completedTasks
            };

            return View("~/Views/HouseKeeping/HouseKeeping.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCleaned(int id)
        {
            await _service.MarkAsCleanedAsync(id);
            return RedirectToAction("Index");
        }
    }
}