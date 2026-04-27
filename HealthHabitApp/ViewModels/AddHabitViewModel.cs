using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthHabitApp.Models;
using HealthHabitApp.Services;
using System.Collections.ObjectModel;

namespace HealthHabitApp.ViewModels
{
    public partial class AddHabitViewModel : BaseViewModel
    {
        private readonly IAppNotificationService _notificationService;
        private readonly IDatabaseService _database;

        [ObservableProperty]
        private string habitName = string.Empty;

        [ObservableProperty]
        private string selectedFrequency = "Daily";

        [ObservableProperty]
        private TimeSpan reminderTime = new TimeSpan(8, 0, 0);

        [ObservableProperty]
        private bool notificationsEnabled = true;

        [ObservableProperty]
        private string nameError = string.Empty;

        [ObservableProperty]
        private bool hasNameError = false;

        [ObservableProperty]
        private bool isSaved = false;

        [ObservableProperty]
        private int editingHabitId = 0;

        [ObservableProperty]
        private string pageTitle = "New Habit";

        [ObservableProperty]
        private string saveBtnText = "Save Habit";

        public ObservableCollection<string> FrequencyOptions { get; } = new()
        {
            "Daily", "Weekdays", "Weekends", "Weekly"
        };

        public ObservableCollection<EmojiOption> EmojiOptions { get; } = new()
        {
            new EmojiOption { Emoji = "🏃", Label = "Exercise" },
            new EmojiOption { Emoji = "📚", Label = "Reading" },
            new EmojiOption { Emoji = "💧", Label = "Hydration" },
            new EmojiOption { Emoji = "🧘", Label = "Mindfulness" },
            new EmojiOption { Emoji = "😴", Label = "Sleep" },
            new EmojiOption { Emoji = "🥗", Label = "Nutrition" },
            new EmojiOption { Emoji = "💊", Label = "Medicine" },
            new EmojiOption { Emoji = "✨", Label = "Other" },
        };

        [ObservableProperty]
        private EmojiOption selectedEmoji;

        public AddHabitViewModel()
        {
            Title = "Add Habit";
            SelectedEmoji = EmojiOptions[7];
        }

        public AddHabitViewModel(IAppNotificationService notificationService, IDatabaseService database)
        {
            _notificationService = notificationService;
            _database = database;
            Title = "Add Habit";
            SelectedEmoji = EmojiOptions[7];
        }

        public void PrepareForEdit(Habit habit)
        {
            EditingHabitId = habit.Id;
            HabitName = habit.Name;
            SelectedFrequency = habit.Frequency ?? "Daily";
            ReminderTime = habit.ReminderTime;
            PageTitle = "Edit Habit";
            SaveBtnText = "Update Habit";
        }

        [RelayCommand]
        private async Task SaveHabit()
        {
            if (!Validate()) return;

            try
            {
                IsBusy = true;
                var habit = new Habit
                {
                    Id = EditingHabitId,
                    Name = HabitName.Trim(),
                    Frequency = SelectedFrequency,
                    ReminderTime = ReminderTime,
                    IsActive = true,
                    CreatedAt = EditingHabitId == 0 ? DateTime.UtcNow : DateTime.UtcNow
                };

                if (_database != null)
                {
                    // save and obtain id (DatabaseService returns the id)
                    var id = await _database.SaveHabitAsync(habit);
                    if (id > 0) habit.Id = id;
                }

                if (_notificationService != null)
                {
                    // try to request permission (harmless on platforms where not required)
                    await _notificationService.RequestPermissionsAsync();

                    // if editing an existing habit, cancel previous reminder first to avoid duplicates
                    if (EditingHabitId > 0)
                    {
                        _notificationService.CancelReminder(habit.Id);
                    }

                    if (NotificationsEnabled)
                    {
                        await _notificationService.ScheduleDailyHabitReminderAsync(
                            habit.Id > 0 ? habit.Id : 1,
                            "Habit Reminder",
                            $"Time to complete: {habit.Name}",
                            ReminderTime);
                    }
                }

                IsSaved = true;
                await Shell.Current.DisplayAlert("Success", $"'{habit.Name}' saved!", "OK");
                ResetForm();
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void SelectEmoji(EmojiOption option)
        {
            if (SelectedEmoji != null) SelectedEmoji.IsSelected = false;
            SelectedEmoji = option;
            option.IsSelected = true;
        }

        private bool Validate()
        {
            HasNameError = string.IsNullOrWhiteSpace(HabitName);
            NameError = HasNameError ? "Please enter a habit name." : string.Empty;
            return !HasNameError;
        }

        private void ResetForm()
        {
            HabitName = string.Empty;
            SelectedFrequency = "Daily";
            ReminderTime = new TimeSpan(8, 0, 0);
            NotificationsEnabled = true;
            EditingHabitId = 0;
            PageTitle = "New Habit";
            SaveBtnText = "Save Habit";
            HasNameError = false;
        }
    }

    public partial class EmojiOption : ObservableObject
    {
        public string Emoji { get; set; }
        public string Label { get; set; }

        [ObservableProperty]
        private bool isSelected;
    }
}
