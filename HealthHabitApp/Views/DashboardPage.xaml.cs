using HealthHabitApp.ViewModels;

namespace HealthHabitApp.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = new DashboardViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DashboardViewModel vm)
            await vm.LoadHabitsAsync();
    }
}
