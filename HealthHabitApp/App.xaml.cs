using HealthHabitApp.Services;

namespace HealthHabitApp
{
    public partial class App : Application
    {

        public static IDatabaseService DatabaseService { get; private set; }

        public App()
        {
            InitializeComponent();

            DatabaseService = new DatabaseService();
            _ = DatabaseService.InitializeAsync();

            MainPage = new AppShell();
        }
    }
}
