using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthHabitApp.Models;
using HealthHabitApp.Services;
using System.Collections.ObjectModel;

namespace HealthHabitApp.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel
    {
        private readonly IDatabaseService _database;

        [ObservableProperty]
        private string greetingText = "Good morning!";

        [ObservableProperty]
        private string dateText = DateTime.Now.ToString("dddd, MMMM d");

        [ObservableProperty]
        private int completedCount = 0;

        [ObservableProperty]
        private int totalCount = 0;

        [ObservableProperty]
        private double completionProgress = 0.0;

        [ObservableProperty]
        private string progressText = "0 of 0 habits done";

        [ObservableProperty]
        private string userName = "Friend";

        public ObservableCollection<HabitDisplayItem> TodayHabits { get; } = new();

        public DashboardViewModel()
        {
            Title = "Dashboard";
            UpdateGreeting();
            LoadSampleData();
        }

        public DashboardViewModel(IDatabaseService database)
        {
            _database = database;
            Title = "Dashboard";
            UpdateGreeting();
        }

        private void UpdateGreeting()
        {
            var hour = DateTime.Now.Hour;
            GreetingText = hour < 12 ? "Good morning" : hour < 17 ? "Good afternoon" : "Good evening";
            DateText = DateTime.Now.ToString("dddd, MMMM d");
        }

        // Sample data for UI demonstration when DB not yet wired
        private void LoadSampleData()
        {
            var sample = new List<HabitDisplayItem>
            {
                new HabitDisplayItem { Id = 1, Name = "Morning Run", FrequencyLabel = "Daily", ReminderTimeLabel = "7:00 AM", CurrentStreak = 5, IsCompleted = true, Icon = "🏃" },
                new HabitDisplayItem { Id = 2, Name = "Read 30 mins", FrequencyLabel = "Daily", ReminderTimeLabel = "9:00 PM", CurrentStreak = 12, IsCompleted = false, Icon = "📚" },
                new HabitDisplayItem { Id = 3, Name = "Drink 8 glasses", FrequencyLabel = "Daily", ReminderTimeLabel = "8:00 AM", CurrentStreak = 3, IsCompleted = false, Icon = "💧" },
                new HabitDisplayItem { Id = 4, Name = "Meditate", FrequencyLabel = "Daily", ReminderTimeLabel = "6:30 AM", CurrentStreak = 0, IsCompleted = true, Icon = "🧘" },
            };

            TodayHabits.Clear();
            foreach (var h in sample)
                TodayHabits.Add(h);

            RefreshStats();
        }

        public async Task LoadHabitsAsync()
        {
            if (_database == null) { LoadSampleData(); return; }
            try
            {
                IsBusy = true;
                var habits = await _database.GetHabitsAsync();
                var profile = await _database.GetUserProfileAsync();
                UserName = profile?.Name ?? "Friend";

                TodayHabits.Clear();
                foreach (var h in habits.Where(h => h.IsActive))
                {
                    TodayHabits.Add(new HabitDisplayItem
                    {
                        Id = h.Id,
                        Name = h.Name,
                        FrequencyLabel = h.Frequency ?? "Daily",
                        ReminderTimeLabel = DateTime.Today.Add(h.ReminderTime).ToString("h:mm tt"),
                        CurrentStreak = h.CurrentStreak,
                        IsCompleted = h.LastCompletedDate?.Date == DateTime.Today,
                        Icon = GetHabitIcon(h.Name)
                    });
                }
                RefreshStats();
            }
            finally { IsBusy = false; }
        }

        private void RefreshStats()
        {
            CompletedCount = TodayHabits.Count(h => h.IsCompleted);
            TotalCount = TodayHabits.Count;
            CompletionProgress = TotalCount > 0 ? (double)CompletedCount / TotalCount : 0;
            ProgressText = $"{CompletedCount} of {TotalCount} habits done";
        }

        [RelayCommand]
        private async Task ToggleHabit(HabitDisplayItem item)
        {
            if (item == null) return;
            item.IsCompleted = !item.IsCompleted;
            // In full app: update DB here
            RefreshStats();
        }

        [RelayCommand]
        private async Task NavigateToAddHabit()
        {
            await Shell.Current.GoToAsync("//addhabit");
        }

        private string GetHabitIcon(string name)
        {
            var n = name?.ToLower() ?? "";
            if (n.Contains("run") || n.Contains("walk") || n.Contains("exercise")) return "🏃";
            if (n.Contains("read") || n.Contains("book")) return "📚";
            if (n.Contains("water") || n.Contains("drink")) return "💧";
            if (n.Contains("meditat")) return "🧘";
            if (n.Contains("sleep")) return "😴";
            if (n.Contains("diet") || n.Contains("eat") || n.Contains("food")) return "🥗";
            return "✨";
        }
    }

    public partial class HabitDisplayItem : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FrequencyLabel { get; set; }
        public string ReminderTimeLabel { get; set; }
        public int CurrentStreak { get; set; }
        public string Icon { get; set; } = "✨";

        [ObservableProperty]
        private bool isCompleted;

        public string StreakText => CurrentStreak > 0 ? $"🔥 {CurrentStreak}" : "Start today";
        public bool HasStreak => CurrentStreak > 0;
    }
}
