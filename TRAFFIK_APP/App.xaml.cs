namespace TRAFFIK_APP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Check if user is already logged in
            var userId = SecureStorage.GetAsync("user_id").Result;
            
            if (!string.IsNullOrEmpty(userId))
            {
                // User is logged in, go to main shell with TabBar
                return new Window(new MainShell());
            }
            else
            {
                // User is not logged in, show login page
                return new Window(new AppShell());
            }
        }
    }
}