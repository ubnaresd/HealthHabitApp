using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthHabitApp.Models;
using HealthHabitApp.Services;
using System.Collections.ObjectModel;

namespace HealthHabitApp.ViewModels
{
    public partial class CalendarViewModel : BaseViewModel
    {
        private readonly IDatabaseService _database;

        [ObservableProperty]
        private string monthYearLabel = DateTime.Now.ToString("MMMM yyyy");

        [ObservableProperty]
        private int totalHabitsThisMonth = 0;

        [ObservableProperty]
        private int completedThisMonth = 0;

        [ObservableProperty]
        private double monthCompletionRate = 0;

        [ObservableProperty]
        private string completionRateText = "0%";

        [ObservableProperty]
        private int longestStreak = 0;

        public ObservableCollection<CalendarDay> CalendarDays { get; } = new();
        public ObservableCollection<HabitStat> HabitStats { get; } = new();

        private DateTime _currentMonth = DateTime.Now;

        public CalendarViewModel()
        {
            Title = "History";
            BuildCalendar();
            LoadSampleStats();
        }

        public CalendarViewModel(IDatabaseService database)
        {
            _database = database;
            Title = "History";
            BuildCalendar();
        }

        private void BuildCalendar()
        {
            CalendarDays.Clear();
            var firstDay = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);
            var startOffset = (int)firstDay.DayOfWeek;

            // Add empty slots for offset
            for (int i = 0; i < startOffset; i++)
                CalendarDays.Add(new CalendarDay { IsEmpty = true });

            // Add actual days
            for (int d = 1; d <= daysInMonth; d++)
            {
                var date = new DateTime(_currentMonth.Year, _currentMonth.Month, d);
                CalendarDays.Add(new CalendarDay
                {
                    Day = d,
                    Date = date,
                    IsToday = date.Date == DateTime.Today,
                    IsFuture = date.Date > DateTime.Today,
                    // Sample: mark some days as completed for demo
                    IsCompleted = date.Date < DateTime.Today && d % 3 != 0
                });
            }

            MonthYearLabel = _currentMonth.ToString("MMMM yyyy");
        }

        private void LoadSampleStats()
        {
            var completed = CalendarDays.Count(d => !d.IsEmpty && d.IsCompleted);
            var total = CalendarDays.Count(d => !d.IsEmpty && !d.IsFuture);
            CompletedThisMonth = completed;
            TotalHabitsThisMonth = total;
            MonthCompletionRate = total > 0 ? (double)completed / total : 0;
            CompletionRateText = $"{(int)(MonthCompletionRate * 100)}%";
            LongestStreak = 7; // Sample

            HabitStats.Clear();
            HabitStats.Add(new HabitStat { Name = "Morning Run", CompletionRate = 0.85, Streak = 5, Icon = "🏃" });
            HabitStats.Add(new HabitStat { Name = "Read 30 mins", CompletionRate = 0.92, Streak = 12, Icon = "📚" });
            HabitStats.Add(new HabitStat { Name = "Drink 8 glasses", CompletionRate = 0.70, Streak = 3, Icon = "💧" });
            HabitStats.Add(new HabitStat { Name = "Meditate", CompletionRate = 0.60, Streak = 0, Icon = "🧘" });
        }

        [RelayCommand]
        private void PreviousMonth()
        {
            _currentMonth = _currentMonth.AddMonths(-1);
            BuildCalendar();
            LoadSampleStats();
        }

        [RelayCommand]
        private void NextMonth()
        {
            if (_currentMonth.Month < DateTime.Now.Month || _currentMonth.Year < DateTime.Now.Year)
            {
                _currentMonth = _currentMonth.AddMonths(1);
                BuildCalendar();
                LoadSampleStats();
            }
        }
    }

    public class CalendarDay
    {
        public int Day { get; set; }
        public DateTime Date { get; set; }
        public bool IsToday { get; set; }
        public bool IsFuture { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsEmpty { get; set; }
        public Color BackgroundColor =>
            IsEmpty ? Colors.Transparent :
            IsToday ? Color.FromArgb("#1DB954") :
            IsFuture ? Color.FromArgb("#F3F4F6") :
            IsCompleted ? Color.FromArgb("#E8F8EF") : Color.FromArgb("#FEE2E2");
        public Color TextColor =>
            IsEmpty ? Colors.Transparent :
            IsToday ? Colors.White :
            IsFuture ? Color.FromArgb("#9CA3AF") :
            IsCompleted ? Color.FromArgb("#1DB954") : Color.FromArgb("#EF4444");
    }

    public class HabitStat
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public double CompletionRate { get; set; }
        public int Streak { get; set; }
        public string CompletionText => $"{(int)(CompletionRate * 100)}%";
        public string StreakText => Streak > 0 ? $"🔥 {Streak} day streak" : "No streak";
    }
}
