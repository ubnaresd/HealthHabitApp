using System.Collections.Generic;
using System.Threading.Tasks;
using HealthHabitApp.Models;

namespace HealthHabitApp.Services
{
    public interface IDatabaseService
    {
        Task InitializeAsync();

        Task<List<Habit>> GetHabitsAsync();
        Task<Habit> GetHabitByIdAsync(int id);
        Task<int> SaveHabitAsync(Habit habit);
        Task<int> DeleteHabitAsync(Habit habit);

        Task<UserProfile> GetUserProfileAsync();
        Task<int> SaveUserProfileAsync(UserProfile profile);
    }
}
