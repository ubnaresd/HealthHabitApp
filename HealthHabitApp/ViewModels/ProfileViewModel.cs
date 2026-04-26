using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthHabitApp.Models;
using HealthHabitApp.Services;

namespace HealthHabitApp.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly IDatabaseService _database;

        [ObservableProperty]
        private string userName = "Friend";

        [ObservableProperty]
        private int dailyGoal = 3;

        [ObservableProperty]
        private int totalHabits = 0;

        [ObservableProperty]
        private int currentStreak = 0;

        [ObservableProperty]
        private int longestStreak = 0;

        [ObservableProperty]
        private double overallCompletionRate = 0;

        [ObservableProperty]
        private string completionRateText = "0%";

        [ObservableProperty]
        private string memberSinceText = "Today";

        [ObservableProperty]
        private bool isEditing = false;

        [ObservableProperty]
        private string editUserName = string.Empty;

        [ObservableProperty]
        private int editDailyGoal = 3;

        [ObservableProperty]
        private string avatarInitial = "F";

        private UserProfile _profile;

        public ProfileViewModel()
        {
            Title = "Profile";
            LoadSampleProfile();
        }

        public ProfileViewModel(IDatabaseService database)
        {
            _database = database;
            Title = "Profile";
        }

        private void LoadSampleProfile()
        {
            UserName = "Alex";
            DailyGoal = 4;
            TotalHabits = 4;
            CurrentStreak = 7;
            LongestStreak = 14;
            OverallCompletionRate = 0.78;
            CompletionRateText = "78%";
            MemberSinceText = "Jan 2025";
            AvatarInitial = "A";
        }

        public async Task LoadProfileAsync()
        {
            if (_database == null) { LoadSampleProfile(); return; }
            try
            {
                IsBusy = true;
                _profile = await _database.GetUserProfileAsync();
                if (_profile != null)
                {
                    UserName = _profile.Name ?? "Friend";
                    DailyGoal = _profile.DailyHabitGoal;
                    MemberSinceText = _profile.CreatedAt.ToString("MMM yyyy");
                    AvatarInitial = UserName.Length > 0 ? UserName[0].ToString().ToUpper() : "?";
                }

                var habits = await _database.GetHabitsAsync();
                TotalHabits = habits.Count;
                // Streak and completion rate would come from Person B's logic
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private void StartEditing()
        {
            EditUserName = UserName;
            EditDailyGoal = DailyGoal;
            IsEditing = true;
        }

        [RelayCommand]
        private async Task SaveProfile()
        {
            if (string.IsNullOrWhiteSpace(EditUserName)) return;
            UserName = EditUserName.Trim();
            DailyGoal = EditDailyGoal < 1 ? 1 : EditDailyGoal > 20 ? 20 : EditDailyGoal;
            AvatarInitial = UserName[0].ToString().ToUpper();
            IsEditing = false;

            if (_database != null && _profile != null)
            {
                _profile.Name = UserName;
                _profile.DailyHabitGoal = DailyGoal;
                await _database.SaveUserProfileAsync(_profile);
            }
        }

        [RelayCommand]
        private void CancelEdit()
        {
            IsEditing = false;
        }
    }
}
