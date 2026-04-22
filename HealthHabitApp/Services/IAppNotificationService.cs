namespace HealthHabitApp.Services;

public interface IAppNotificationService
{
    // Requests permissions from the user (Required for Android 13+ and iOS)
    Task<bool> RequestPermissionsAsync();

    // Schedules a daily repeating notification
    Task ScheduleDailyHabitReminderAsync(int habitId, string title, string description, TimeSpan timeOfDay);

    // Cancels a specific notification using the Habit's ID
    void CancelReminder(int habitId);
}