using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.ViewModels
{
    public class AdminProfileViewModel : BaseViewModel
    {
        private readonly SessionService _session;

        public string FullName => _session.UserName;
        public string Email => _session.CurrentUser?.Email ?? "N/A";
        public string PhoneNumber => _session.CurrentUser?.PhoneNumber ?? "N/A";

        public Command LogoutCommand { get; }

        public AdminProfileViewModel(SessionService session)
        {
            _session = session;
            LogoutCommand = new Command(() => ExecuteSafeAsync(LogoutAsync, "Logging out..."));
        }

        private async Task LogoutAsync()
        {
            try
            {
                _session.Clear();

                SecureStorage.Remove("user_id");
                SecureStorage.Remove("user_name");
                SecureStorage.Remove("role_id");
                SecureStorage.Remove("email");

                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during logout: {ex.Message}";
            }
        }
    }
}

