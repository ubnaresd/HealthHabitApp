using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HealthHabitApp.Models;
using SQLite;

namespace HealthHabitApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection _database;
        private bool _initialized;

        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            var dbPath = GetDatabasePath("healthhabitapp.db3");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<Habit>();
            await _database.CreateTableAsync<UserProfile>();

            // Seed sample habits if none exist
            var count = await _database.Table<Habit>().CountAsync();
            if (count == 0)
            {
                var seedDate = DateTime.UtcNow.AddMonths(-1);
                var today = DateTime.UtcNow.Date;
                var samples = new List<Habit>
                {
                    new Habit { Name = "Morning Run", Frequency = "Daily", ReminderTime = new TimeSpan(7,0,0), IsActive = true, CurrentStreak = 5, CreatedAt = seedDate, LastCompletedDate = today },
                    new Habit { Name = "Read 30 mins", Frequency = "Daily", ReminderTime = new TimeSpan(21,0,0), IsActive = true, CurrentStreak = 12, CreatedAt = seedDate, LastCompletedDate = today.AddDays(-1) },
                    new Habit { Name = "Drink 8 glasses", Frequency = "Daily", ReminderTime = new TimeSpan(8,0,0), IsActive = true, CurrentStreak = 3, CreatedAt = seedDate, LastCompletedDate = today.AddDays(-2) },
                    new Habit { Name = "Meditate", Frequency = "Daily", ReminderTime = new TimeSpan(6,30,0), IsActive = true, CurrentStreak = 0, CreatedAt = seedDate, LastCompletedDate = null }
                };

                foreach (var s in samples)
                {
                    await _database.InsertAsync(s);
                }
            }

            _initialized = true;
        }

        private string GetDatabasePath(string filename)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folder, filename);
        }

        public async Task<List<Habit>> GetHabitsAsync()
        {
            await InitializeAsync();
            return await _database.Table<Habit>().ToListAsync();
        }

        public async Task<Habit> GetHabitByIdAsync(int id)
        {
            await InitializeAsync();
            return await _database.Table<Habit>().FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<int> SaveHabitAsync(Habit habit)
        {
            await InitializeAsync();

            if (habit.Id == 0)
            {
                var result = await _database.InsertAsync(habit);
                // After insert, SQLite-net populates the Id on the object.
                return habit.Id;
            }
            else
            {
                await _database.UpdateAsync(habit);
                return habit.Id;
            }
        }

        public async Task<int> DeleteHabitAsync(Habit habit)
        {
            await InitializeAsync();
            return await _database.DeleteAsync(habit);
        }

        public async Task<UserProfile> GetUserProfileAsync()
        {
            await InitializeAsync();

            var profile = await _database.Table<UserProfile>().FirstOrDefaultAsync();

            if (profile == null)
            {
                profile = new UserProfile
                {
                    Name = "You",
                    DailyHabitGoal = 3
                };
                await _database.InsertAsync(profile);
            }

            return profile;
        }

        public async Task<int> SaveUserProfileAsync(UserProfile profile)
        {
            await InitializeAsync();

            if (profile.Id == 0)
                return await _database.InsertAsync(profile);
            else
                return await _database.UpdateAsync(profile);

        }

        // Small helper to delete all habits - useful for demo/cleanup
        public async Task<int> DeleteAllHabitsAsync()
        {
            await InitializeAsync();
            return await _database.ExecuteAsync("DELETE FROM Habits");
        }

        public async Task ClearAllDataAsync()
        {
            await InitializeAsync();
            await _database.ExecuteAsync("DELETE FROM Habits");
            await _database.ExecuteAsync("DELETE FROM UserProfile");
        }
    }
}
