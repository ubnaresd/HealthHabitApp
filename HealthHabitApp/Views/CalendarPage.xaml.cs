using HealthHabitApp.ViewModels;

namespace HealthHabitApp.Views;

public partial class CalendarPage : ContentPage
{
    public CalendarPage()
    {
        InitializeComponent();
        BindingContext = new CalendarViewModel(App.DatabaseService);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CalendarViewModel vm)
            await vm.LoadStatsAsync();
    }
}
