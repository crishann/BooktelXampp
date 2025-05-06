// Services/RoomTaskOperations.cs
using Microsoft.EntityFrameworkCore;
using NewBooktel.Data; // Assuming you have a DataContext
using NewBooktel.Models;
using System.Collections.Generic;
using System.Linq; // Add this for the Where() method
using System.Threading.Tasks;

namespace NewBooktel.Services
{
    public class RoomTaskOperations
    {
        private readonly ApplicationDbContext _dbContext;

        public RoomTaskOperations(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<RoomTask>> GetPendingAsync()
        {
            return await _dbContext.RoomTasks
                .Where(t => t.Status == "Pending" || t.Status == "In Progress")
                .ToListAsync();
        }

        public async Task<List<RoomTask>> GetCompletedAsync()
        {
            return await _dbContext.RoomTasks
                .Where(t => t.Status == "Cleaned")
                .ToListAsync();
        }

        public async Task MarkAsCleanedAsync(int id)
        {
            var task = await _dbContext.RoomTasks.FindAsync(id);
            if (task != null)
            {
                task.Status = "Cleaned";
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
