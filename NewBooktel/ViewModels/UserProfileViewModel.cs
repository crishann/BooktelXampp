using System.Collections.Generic;
using NewBooktel.Models;

namespace NewBooktel.ViewModels
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}