using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthHabitApp.Services;

namespace HealthHabitApp.ViewModels;

public partial class AddHabitViewModel : ObservableObject
{
    private readonly IAppNotificationService _notificationService;
    // Database repository would be injected here too

    [ObservableProperty]
    private string habitTitle;

    [ObservableProperty]
    private TimeSpan reminderTime;

    // Inject the service through the constructor
    public AddHabitViewModel(IAppNotificationService notificationService)
    {
        _notificationService = notificationService;
        ReminderTime = new TimeSpan(8, 0, 0); // Default to 8:00 AM
    }

    [RelayCommand]
    public async Task SaveHabitAsync()
    {
        // 1. Person B's logic: Save to SQLite Database and get the new ID back
        // int newHabitId = await _database.SaveHabitAsync(newHabit);
        int mockHabitId = 1; // Placeholder for the actual DB ID

        // 2. YOUR logic: Schedule the notification
        await _notificationService.ScheduleDailyHabitReminderAsync(
            mockHabitId,
            "Habit Reminder",
            $"Time to complete: {HabitTitle}",
            ReminderTime);

        // Navigate back...
    }
}