using HealthHabitApp.Views;

namespace HealthHabitApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register modal route for Add/Edit Habit page
        Routing.RegisterRoute("addhabit", typeof(AddHabitPage));
    }
}
