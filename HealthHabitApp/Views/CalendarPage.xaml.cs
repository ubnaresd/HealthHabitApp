using HealthHabitApp.ViewModels;

namespace HealthHabitApp.Views;

public partial class CalendarPage : ContentPage
{
    public CalendarPage()
    {
        InitializeComponent();
        BindingContext = new CalendarViewModel();
    }
}
