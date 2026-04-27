using HealthHabitApp.ViewModels;

namespace HealthHabitApp.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        // use the app's shared database service if available
        BindingContext = new DashboardViewModel(App.DatabaseService);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DashboardViewModel vm)
            await vm.LoadHabitsAsync();
    }
}
