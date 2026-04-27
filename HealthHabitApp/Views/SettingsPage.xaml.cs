using HealthHabitApp.ViewModels;

namespace HealthHabitApp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        var notif = App.Services?.GetService(typeof(HealthHabitApp.Services.IAppNotificationService)) as HealthHabitApp.Services.IAppNotificationService;
        BindingContext = new SettingsViewModel(notif, App.DatabaseService);
    }
}
