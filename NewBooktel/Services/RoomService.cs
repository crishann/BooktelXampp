using Microsoft.EntityFrameworkCore;
using NewBooktel.Data;
using NewBooktel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewBooktel.Services
{
    public class RoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
    }
}