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
                return await _database.InsertAsync(habit);
            else
                return await _database.UpdateAsync(habit);
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
    }
}
