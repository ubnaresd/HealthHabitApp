using HealthHabitApp.Services;
using System;

namespace HealthHabitApp
{
    public partial class App : Application
    {

        public static IDatabaseService DatabaseService { get; private set; }
        // Expose the built service provider so pages/viewmodels can resolve services when needed
        public static IServiceProvider Services { get; internal set; }

        public App()
        {
            InitializeComponent();

            DatabaseService = new DatabaseService();
            _ = DatabaseService.InitializeAsync();

            MainPage = new AppShell();
        }
    }
}
