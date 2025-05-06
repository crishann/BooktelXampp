using System.Collections.Generic;
using NewBooktel.Models;

namespace NewBooktel.Models
{
    public class HousekeepingViewModel
    {
        public IEnumerable<RoomTask> PendingTasks { get; set; }
        public IEnumerable<RoomTask> CompletedTasks { get; set; }
    }
}