using HealthHabitApp.ViewModels;

namespace HealthHabitApp.Views;

public partial class AddHabitPage : ContentPage
{
    public AddHabitPage()
    {
        InitializeComponent();
        // Use the app's shared DatabaseService so saved habits persist
        BindingContext = new AddHabitViewModel(null, App.DatabaseService);
    }
}
