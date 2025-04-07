namespace NewBooktel.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserId { get; set; } // You can set this later from session or auth
        public int RoomId { get; set; } // We'll map RoomType to RoomId in controller
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Guest { get; set; }
        public string RoomType { get; set; }

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
