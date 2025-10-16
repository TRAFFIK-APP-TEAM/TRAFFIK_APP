using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Views;
using TRAFFIK_APP.Models.Dtos.Auth;
using TRAFFIK_APP.Models.Entities.User;

namespace TRAFFIK_APP.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly AuthClient _authClient;
        private readonly SessionService _session;

        private string _email = string.Empty;
        private string _password = string.Empty;
        private bool _isPasswordHidden = true;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set => SetProperty(ref _isPasswordHidden, value);
        }

        public Command LoginCommand { get; }

        public LoginViewModel(AuthClient authClient, SessionService session)
        {
            _authClient = authClient;
            _session = session;
            LoginCommand = new Command(() => ExecuteSafeAsync(LoginAsync, "Logging in..."));

            // Log that ViewModel was created
            System.Diagnostics.Debug.WriteLine($"[LoginViewModel] Created successfully");
            Console.WriteLine($"[LoginViewModel] Created successfully"); // Also write to Console
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Email and Password are required.";
                return;
            }
            var dto = new UserLoginDto
            {
                Email = Email,
                Password = Password
            };
            var response = await _authClient.LoginAsync(dto);
            if (response is not null)
            {
                await OnLoginSuccess(response);
            }
            else
            {
                ErrorMessage = "Login failed. Please check your credentials.";
            }
        }

        public async Task OnLoginSuccess(UserLoginResponseDto response)
        {
            var user = User.FromLoginResponse(response);
            _session.SetUser(user);

            // Keep the session
            await SecureStorage.SetAsync("user_id", user.Id.ToString());
            await SecureStorage.SetAsync("user_name", user.FullName);
            await SecureStorage.SetAsync("role_id", user.RoleId.ToString());
            await SecureStorage.SetAsync("email", user.Email);

            // Clear sensitive data from the form
            Password = string.Empty;

            // Switch to the main app shell with TabBar
            switch (user.RoleId)
            {
                case 1:
                    await Shell.Current.GoToAsync("//AdminDashboardPage");
                    break;
                case 2:
                    await Shell.Current.GoToAsync("//StaffDashboardPage");
                    break;
                default:
                case 3:
                    await Shell.Current.GoToAsync("//UserDashboardPage");
                    break;
            }
        }
    }
}