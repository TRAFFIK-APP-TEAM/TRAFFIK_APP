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
                // User is logged in, go to AppShell with TabBar
                return new Window(new AppShell());
            }
            else
            {
                // User is not logged in, navigate to LoginPage inside AppShell
                var shell = new AppShell();
                shell.GoToAsync("//LoginPage"); // Ensure LoginPage route is registered in AppShell
                return new Window(shell);
            }
        }
    }
}