using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MySqlConnector;
using NewBooktel.Models;

[Authorize(Roles = "admin")] // ✅ Restrict Access to Admins Only using Attribute
public class AdminController : Controller
{
    // ✅ Removed the manual IsAdmin check as [Authorize] handles it
    private readonly string connectionString = "server=localhost;database=bookteldb;user=root;password=;";

    public async Task<IActionResult> Feedback()
    {
        var feedbackList = new List<ContactUs>();

        using (var connection = new MySqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT * FROM contactus";

            using (var cmd = new MySqlCommand(query, connection))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    feedbackList.Add(new ContactUs
                    {
                        Contid = reader.GetInt32("Contid"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        Message = reader.GetString("message")
                    });
                }
            }
        }

        return View(feedbackList);
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

        return View();
    }
    private readonly string _connectionString;

    public AdminController(IConfiguration configuration)
    {
        // Get the connection string from appsettings.json
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    public async Task<IActionResult> BookingReq()
    {
        List<Booking> bookings = new List<Booking>();

        // Connect to the database
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Query to retrieve data from the 'bookings' table
            var query = "SELECT * FROM bookings";

            // Create the command and execute it
            using var cmd = new MySqlCommand(query, connection);
            using var reader = await cmd.ExecuteReaderAsync();

            // Loop through the results and map them to the Booking model
            while (await reader.ReadAsync())
            {
                bookings.Add(new Booking
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    CheckInDate = Convert.ToDateTime(reader["CheckInDate"]),
                    CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]),
                    Guest = Convert.ToInt32(reader["Guest"]),
                    RoomType = reader["RoomType"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    Email = reader["Email"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    Address = reader["Address"].ToString(),
                    SpecialRequests = reader["SpecialRequests"].ToString(),
                    PaymentMethod = reader["PaymentMethod"].ToString(),
                    PaymentStatus = reader["PaymentStatus"].ToString(),
                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                    Status = reader["Status"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                });
            }
        }

        return View("~/Views/Admin/BookingReq.cshtml", bookings);
    }

    //public IActionResult Feedback()
    //{
    //    return View();
    //}

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