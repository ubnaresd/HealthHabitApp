using System;

namespace HealthHabitApp.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan ReminderTime { get; set; }
        public string Frequency { get; set; }
        public bool IsActive { get; set; } = true;
        public int CurrentStreak { get; set; }
        public DateTime? LastCompletedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
