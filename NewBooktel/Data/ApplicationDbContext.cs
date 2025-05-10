using Microsoft.EntityFrameworkCore;
using NewBooktel.Models;

namespace NewBooktel.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Define DbSet properties for each model
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
      
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RoomTask> RoomTasks { get; set; }
       

        // Optional: Add OnModelCreating method for custom configurations (if necessary)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Example: Configure User table
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id); // If you have any custom primary key or constraints

            // Example: Configure Booking table if any custom configurations
            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasDefaultValue("Pending"); // Set default values or other custom configurations
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>()
                .Property(r => r.status)
                .HasConversion<string>();
            // Add other entity configurations as necessary
        }
    }
}
