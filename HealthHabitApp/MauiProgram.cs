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

        // Register services
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IAppNotificationService, NotificationService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // store the service provider for use by code-behind pages that need to resolve services
        App.Services = app.Services;

        return app;
    }
}