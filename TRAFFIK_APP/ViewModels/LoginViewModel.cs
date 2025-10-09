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
            // Validate input first
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter both email and password.";
                return;
            }

            // Try to login directly - let the actual error bubble up
            var dto = new UserLoginDto { Email = Email, Password = Password };
            
            // Show a debug message
            await Application.Current.MainPage.DisplayAlert("Debug", $"Calling API with Email: {Email}", "OK");
            
            var response = await _authClient.LoginAsync(dto);
            
            // Show what we got back
            if (response is not null)
            {
                await Application.Current.MainPage.DisplayAlert("Debug", 
                    $"Response received!\nID: {response.Id}\nName: {response.FullName}\nEmail: {response.Email}", "OK");
                
                var user = User.FromLoginResponse(response);
                _session.SetUser(user);
                await Shell.Current.GoToAsync(nameof(DashboardPage));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Debug", "Response was NULL from API", "OK");
                ErrorMessage = "Invalid credentials. Please check your email and password.";
            }
        }
    }
}

