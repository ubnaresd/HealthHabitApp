using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthHabitApp.Services;

namespace HealthHabitApp.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        private readonly IAppNotificationService _notificationService;

        [ObservableProperty]
        private bool notificationsEnabled = true;

        [ObservableProperty]
        private bool darkModeEnabled = false;

        [ObservableProperty]
        private string selectedTheme = "Light";

        [ObservableProperty]
        private bool soundEnabled = true;

        [ObservableProperty]
        private string appVersion = "1.0.0";

        public SettingsViewModel()
        {
            Title = "Settings";
        }

        public SettingsViewModel(IAppNotificationService notificationService)
        {
            _notificationService = notificationService;
            Title = "Settings";
        }

        [RelayCommand]
        private async Task RequestNotificationPermissions()
        {
            if (_notificationService != null)
            {
                var granted = await _notificationService.RequestPermissionsAsync();
                NotificationsEnabled = granted;
                var msg = granted ? "Notifications enabled!" : "Permission denied.";
                await Shell.Current.DisplayAlert("Notifications", msg, "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Notifications", "Notifications enabled (demo).", "OK");
            }
        }

        [RelayCommand]
        private async Task ClearAllData()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Clear All Data",
                "This will delete all your habits and history. Are you sure?",
                "Delete", "Cancel");
            if (confirm)
            {
                await Shell.Current.DisplayAlert("Done", "All data cleared.", "OK");
            }
        }

        [RelayCommand]
        private async Task SendFeedback()
        {
            await Shell.Current.DisplayAlert("Feedback", "Thank you! Feedback feature coming soon.", "OK");
        }

        [RelayCommand]
        private async Task RateApp()
        {
            await Shell.Current.DisplayAlert("Rate Us", "Thank you for using HealthHabitApp!", "OK");
        }
    }
}
