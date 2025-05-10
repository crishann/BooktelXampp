using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MySqlConnector;
using NewBooktel.Models;
using NewBooktel.Data;



[Authorize(Roles = "admin")] // ✅ Restrict Access to Admins Only using Attribute
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;

    public AdminController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

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
        ViewBag.RoomTypes = _context.Rooms.Count();
        ViewBag.NewBookings = _context.Bookings.Count(b => b.Status == "Pending");
        ViewBag.ConfirmedBookings = _context.Bookings.Count(b => b.PaymentStatus == "paid");
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


    // GET: /Admin/BookingReq
    public async Task<IActionResult> BookingReq()
    {
        
        List<Booking> bookings = new List<Booking>();
        List<Room> rooms = new List<Room>();

        // First, connect to the database for the bookings query
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Query to retrieve data from the 'bookings' table
            var query = "SELECT * FROM bookings WHERE Status = 'Pending'";
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

        // Now, connect to the database for the rooms query
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Query to retrieve rooms (adjust this based on your needs)
            var roomQuery = "SELECT * FROM rooms WHERE Status = 'Available'";
            using var roomCmd = new MySqlCommand(roomQuery, connection);
            using var roomReader = await roomCmd.ExecuteReaderAsync();

            // Loop through the results and map them to the Room model
            while (await roomReader.ReadAsync())
            {
                var statusString = roomReader["status"].ToString();  // Get status from the database as a string

                // Try to convert the string to a RoomStatus enum
                RoomStatus roomStatus = RoomStatus.Available;  // Default to Available if conversion fails
                if (!Enum.TryParse(statusString, true, out roomStatus))
                {
                    roomStatus = RoomStatus.Available; // Default or fallback status
                }

                rooms.Add(new Room
                {
                    Id = Convert.ToInt32(roomReader["Id"]),
                    room_number = Convert.ToInt32(roomReader["room_number"]),
                    Name = roomReader["Name"].ToString(),
                    Price = Convert.ToDecimal(roomReader["Price"]),
                    status = roomStatus,  // Assign the parsed enum value
                    ImageUrl = roomReader["ImageUrl"].ToString()
                });
            }
        }

        // Create the view model and return the data to the view
        var viewModel = new BookingViewModel
        {
            Bookings = bookings,
            Rooms = rooms
        };

        return View("~/Views/Admin/BookingReq.cshtml", viewModel); // Pass the view model to the view

        
    }

    public IActionResult Back()
    {
        var bookings = _context.Bookings.ToList(); // or wherever you get your bookings
        var model = new BookingViewModel
        {
            Bookings = bookings
        };

        return View("BookingReq", model);
    }



    public IActionResult Approve(int bookingId)
    {
        var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking == null)
        {
            return NotFound();
        }

        var rooms = _context.Rooms.ToList();

        var viewModel = new BookingViewModel
        {
            Booking = booking,
            Rooms = rooms
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult ApproveBooking(int bookingId, int roomId)
    {
        var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking == null)
        {
            return NotFound();
        }

        booking.Status = "Approved";
        booking.Room_id = roomId;

        _context.SaveChanges();

        return RedirectToAction("BookingReq");
    }




    // GET: APPROVE BOOKING!
    // Approve the booking and show room assignment modal
    [HttpPost]
    public IActionResult ApproveBooking(int bookingId)
    {
        // Set booking status to Pending confirmation until room is assigned
        var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking != null)
        {
            // Set status to 'Pending Confirmation'
            booking.Status = "Pending Confirmation";
            _context.SaveChanges();
        }

        // Get available rooms by comparing the RoomStatus enum
        var rooms = _context.Rooms.Where(r => r.status == RoomStatus.Available).ToList();
        ViewBag.Rooms = rooms;

        return View();  // or redirect to another view if needed
    }


    // Cancel the booking
    [HttpPost]
    public IActionResult CancelBooking(int bookingId)
    {
        var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
        if (booking != null)
        {
            booking.Status = "Cancelled";
            _context.SaveChanges();
        }

        return RedirectToAction("BookingRequests"); // Redirect back to the booking list
    }


    // Assign room after booking approval
    [HttpPost]
    public IActionResult AssignRoom(int bookingId, int roomId)
    {
        var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
        var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);

        if (booking != null && room != null)
        {
            // Check if the room is available
            if (room.status == RoomStatus.Available)
            {
                // Update booking status to "Confirmed"
                booking.Status = "Confirmed";
                booking.Room_id = roomId; // Assign the selected room to the booking

                // Update room status to "Occupied"
                room.status = RoomStatus.Occupied;

                _context.SaveChanges();
            }
            else
            {
                // Room is not available, show error message
                TempData["ErrorMessage"] = "The selected room is not available.";
                return RedirectToAction("BookingReq"); // Or another appropriate action
            }
        }

        return RedirectToAction("BookingReq"); // Redirect back to the booking list
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

    // yawa na 
    [HttpGet]
    public IActionResult Housekeeping()
    {
        var tasks = _context.RoomTasks.ToList();
        return View(tasks);
    }

    [HttpPost]
    public IActionResult AssignHousekeepingTask(int roomNumber, string status)
    {
        var task = new RoomTask
        {
            RoomNumber = roomNumber,
            Status = status,
            AssignedDate = DateTime.Today
        };

        _context.RoomTasks.Add(task);
        _context.SaveChanges();

        return RedirectToAction("Housekeeping");
    }

    [HttpPost]
    public IActionResult UpdateTaskStatus(int id, string newStatus)
    {
        var task = _context.RoomTasks.Find(id);
        if (task != null)
        {
            task.Status = newStatus;
            _context.SaveChanges();
        }

        return RedirectToAction("Housekeeping");
    }


    public IActionResult Rooms()
    {
        var rooms = _context.Rooms.ToList(); // or filter logic
        return View(rooms); // This must not be null
    }



    // new
    public IActionResult Payment()
    {
        var confirmedBookings = _context.Bookings
                                       .Where(b => b.PaymentStatus == "Paid")
                                       .ToList();
        return View(confirmedBookings);
    }



    // SPECIAL OFFER
    [HttpGet]
    public IActionResult SpecialOffers()
    {
        var offers = _context.SpecialOffers.ToList();
        return View(offers);
    }

    [HttpPost]
    public IActionResult SpecialOffers(string title, string description, decimal discountPercent, DateTime? startDate, DateTime? endDate, bool isActive)
    {
        var offer = new SpecialOffer
        {
            Title = title,
            Description = description,
            DiscountPercent = discountPercent,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = isActive,
            CreatedAt = DateTime.Now
        };

        _context.SpecialOffers.Add(offer);
        _context.SaveChanges();

        return RedirectToAction("SpecialOffers");
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>

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



    public IActionResult RoomsManagement(string filter = "")
    {
        var rooms = _context.Rooms.AsQueryable();

        // Apply filtering based on the string values for room status
        if (filter == "available")
            rooms = rooms.Where(r => r.status == RoomStatus.Available);
        else if (filter == "occupied")
            rooms = rooms.Where(r => r.status == RoomStatus.Occupied);
        else if (filter == "maintenance")
            rooms = rooms.Where(r => r.status == RoomStatus.Maintenance);

        rooms = rooms.OrderBy(r => r.Name); // Sort by room type

        return View(rooms.ToList()); // Convert to list for passing to the view
    }



}