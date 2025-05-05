using Microsoft.AspNetCore.Mvc;

namespace NewBooktel.Models
{
    
    public class BookingViewModel
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string? RoomType { get; set; }
        public int Guests { get; set; }
    }
}

