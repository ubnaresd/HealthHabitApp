using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthHabitApp.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private bool isBusy;
    }
}
