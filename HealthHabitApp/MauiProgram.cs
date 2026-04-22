using Microsoft.Extensions.Logging;
using HealthHabitApp.Services;

#if ANDROID || IOS
using Plugin.LocalNotification;
#endif

namespace HealthHabitApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if ANDROID || IOS
        builder.UseLocalNotification();
#endif

        // Update this line to use IAppNotificationService
        builder.Services.AddSingleton<IAppNotificationService, NotificationService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}