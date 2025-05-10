using Microsoft.AspNetCore.Mvc;

namespace NewBooktel.Models
{
    
    public class BookingViewModel
    {
        public List<Booking> Bookings { get; set; }
        public List<Room> Rooms { get; set; }
        public Booking Booking { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string? RoomType { get; set; }
        public int Guests { get; set; }

    }
}

