﻿using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewBooktel.Data;
using NewBooktel.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using BCrypt.Net;

public class UserDashController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserDashController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ Reservation Page
    public IActionResult Reservation()
    {
        return View("~/Views/UserDash/Reservation.cshtml");
    }

    // ✅ Profile Page - Get User Data
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        string? userEmail = HttpContext.Session.GetString("UserEmail"); // Allow null
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Home");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return RedirectToAction("Login", "Home");
        }

        return View(user);
    }

    // ✅ Update Profile via AJAX (Recommended)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile([FromForm] string Email, [FromForm] string NewPassword)
    {
        string? userEmail = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(userEmail))
        {
            return Json(new { success = false, message = "Unauthorized access. Please log in." });
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return Json(new { success = false, message = "User not found." });
        }

        // Check if new email is already taken
        if (Email != user.Email && await _context.Users.AnyAsync(u => u.Email == Email))
        {
            return Json(new { success = false, message = "This email is already in use." });
        }

        user.Email = Email;

        if (!string.IsNullOrEmpty(NewPassword))
        {
            user.Password = HashPassword(NewPassword);
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        HttpContext.Session.SetString("UserEmail", user.Email);

        return Json(new { success = true, message = "Profile updated successfully!" });
    }

    // ✅ Secure Password Hashing using BCrypt
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
