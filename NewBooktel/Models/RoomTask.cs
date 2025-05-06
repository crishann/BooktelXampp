namespace NewBooktel.Models
{
    public class RoomTask
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public string Status { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}
