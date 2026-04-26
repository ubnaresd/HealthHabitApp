using HealthHabitApp.ViewModels;

namespace HealthHabitApp.Views;

public partial class AddHabitPage : ContentPage
{
    public AddHabitPage()
    {
        InitializeComponent();
        BindingContext = new AddHabitViewModel();
    }
}
