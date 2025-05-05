using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "admin")] // ✅ Restrict Access to Admins Only using Attribute
public class AdminController : Controller
{
    // ✅ Removed the manual IsAdmin check as [Authorize] handles it

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

        return View();
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