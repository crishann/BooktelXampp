namespace NewBooktel.Models;

public class Room
{
    public int Id { get; set; }
    public int room_number { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    // Using the enum for status, EF will store it as a string in the database
    public RoomStatus status { get; set; } = RoomStatus.Available;

    public string ImageUrl { get; set; } = string.Empty;
}

public enum RoomStatus
{
    Available,
    Occupied,
    Reserved,
    Maintenance
}

