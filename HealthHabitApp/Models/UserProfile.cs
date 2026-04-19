using System;

namespace HealthHabitApp.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DailyHabitGoal { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
