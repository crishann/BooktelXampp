using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

public class AdminController : Controller
{
    // ✅ Restrict Access to Admins Only
    private bool IsAdmin()
    {
        var role = HttpContext.Session.GetString("UserRole") ?? "unknown";
        Console.WriteLine($"📌 DEBUG: Retrieved Session Role = '{role}'"); // ✅ Debugging output
        return role == "admin"; // Ensure lowercase comparison
    }

    public IActionResult AdminIndex()
    {
        ViewBag.RoomTypes = 3;
        ViewBag.NewBookings = 11;
        ViewBag.ConfirmedBookings = 0;
        ViewBag.SpecialOffers = 0;

        // ✅ Use ParseExact to prevent FormatException
        ViewBag.LatestBookings = new List<object>
        {
            new {
                Code = "X880R267",
                RoomType = "Beach Double Room",
                CheckIn = DateTime.ParseExact("06/28/2018", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                CheckOut = DateTime.ParseExact("06/30/2018", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                Total = 300,
                FirstName = "John",
                LastName = "Smith",
                Email = "test@test.com",
                Phone = "212-324-5422"
            },
            new {
                Code = "N970N835",
                RoomType = "Beach Double Room",
                CheckIn = DateTime.ParseExact("06/28/2018", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                CheckOut = DateTime.ParseExact("06/30/2018", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                Total = 300,
                FirstName = "John",
                LastName = "Smith",
                Email = "test@test.com",
                Phone = "212-324-5422"
            }
        };

        var rooms = new List<object>
        {
            new { Name = "Double Deluxe Room", Price = "8,853", ImageUrl = "/images/room1.jpg" },
            new { Name = "Presidential Suite", Price = "8,853", ImageUrl = "/images/room2.jpg" },
            new { Name = "Standard Room", Price = "8,853", ImageUrl = "/images/room3.jpg" },
            new { Name = "Double Room", Price = "8,853", ImageUrl = "/images/room4.jpg" }
        };

        ViewBag.Rooms = rooms;

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

    public IActionResult AddRoom()
    {
        return View();
    }

    public IActionResult EditRoom()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddRoom(string roomName, decimal roomPrice)
    {
        // Your logic for adding a room goes here
        // For example, saving the room to the database

        // Redirect or return a response
        return RedirectToAction("Rooms");  // Redirect back to the rooms page
    }

}
