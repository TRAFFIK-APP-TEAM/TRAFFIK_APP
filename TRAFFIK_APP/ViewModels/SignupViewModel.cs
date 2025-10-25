using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Views;
using TRAFFIK_APP.Models.Dtos.Auth;
using TRAFFIK_APP.Models.Entities;

namespace TRAFFIK_APP.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
        private readonly AuthClient _authClient;
        private readonly SessionService _session;

        private string _name = string.Empty;
        private string _surname = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private bool _isPasswordHidden = true;
        private bool _isConfirmPasswordHidden = true;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set => SetProperty(ref _isPasswordHidden, value);
        }

        public bool IsConfirmPasswordHidden
        {
            get => _isConfirmPasswordHidden;
            set => SetProperty(ref _isConfirmPasswordHidden, value);
        }

        public Command SignupCommand { get; }

        public SignupViewModel(AuthClient authClient, SessionService session)
        {
            _authClient = authClient;
            _session = session;
            SignupCommand = new Command(() => ExecuteSafeAsync(SignupAsync, "Creating account..."));
        }



        private async Task SignupAsync()
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
            {
                ErrorMessage = "Please enter your name and surname.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Please enter your email.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter a password.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            var dto = new UserRegisterDto
            {
                FullName = $"{Name} {Surname}",
                Email = Email,
                Password = Password,
                PhoneNumber = "", // Still must add a phone field
                RoleId = 2 
            };

            var response = await _authClient.RegisterAsync(dto);

            if (response is not null)
            {
                var user = User.FromLoginResponse(response);
                _session.SetUser(user);
                
                // Store session
                await SecureStorage.SetAsync("user_id", user.Id.ToString());
                await SecureStorage.SetAsync("user_name", user.FullName);
                await SecureStorage.SetAsync("role_id", user.RoleId.ToString());
                await SecureStorage.SetAsync("email", user.Email);
                
                // Switch to the main app shell with TabBar
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                ErrorMessage = "Signup failed. Email may already be in use.";
            }
        }
    }
}

