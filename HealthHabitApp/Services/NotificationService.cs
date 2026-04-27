using System;
using System.Threading.Tasks;

#if ANDROID || IOS
using Plugin.LocalNotification;
#endif

namespace HealthHabitApp.Services;

// Change INotificationService to IAppNotificationService
public class NotificationService : IAppNotificationService
{
    public async Task<bool> RequestPermissionsAsync()
    {
#if ANDROID || IOS
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            return await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
#endif
        return true;
    }

    public async Task ScheduleDailyHabitReminderAsync(int habitId, string title, string description, TimeSpan timeOfDay)
    {
#if ANDROID || IOS
        // compute the next occurrence of the reminder time
        var next = DateTime.Today.Add(timeOfDay);
        if (next <= DateTime.Now)
            next = next.AddDays(1);

        var notification = new NotificationRequest
        {
            NotificationId = habitId,
            Title = title,
            Description = description,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = next,
                RepeatType = NotificationRepeat.Daily
            }
        };

        await LocalNotificationCenter.Current.Show(notification);
#else
        await Task.CompletedTask;
#endif
    }

    public void CancelReminder(int habitId)
    {
#if ANDROID || IOS
        LocalNotificationCenter.Current.Cancel(habitId);
#endif
    }
}