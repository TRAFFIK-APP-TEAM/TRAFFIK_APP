using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Models.Dtos.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace TRAFFIK_APP.ViewModels
{
    public class AddAdminViewModel : BaseViewModel
    {
        private readonly AuthClient _authClient;
        private readonly ILogger<AddAdminViewModel> _logger;

        private string _fullName = string.Empty;
        private string _email = string.Empty;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _selectedRole = string.Empty;

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string SelectedRole
        {
            get => _selectedRole;
            set => SetProperty(ref _selectedRole, value);
        }

        public Command CreateAccountCommand { get; }

        public AddAdminViewModel(AuthClient authClient, ILogger<AddAdminViewModel> logger)
        {
            _authClient = authClient;
            _logger = logger;
            CreateAccountCommand = new Command(() => ExecuteSafeAsync(CreateAccountAsync, "Creating account..."));
        }

        private async Task CreateAccountAsync()
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(FullName))
            {
                ErrorMessage = "Please enter a full name.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "Please enter an email address.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Please enter a username.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please enter a password.";
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedRole))
            {
                ErrorMessage = "Please select a role.";
                return;
            }

            // Parse role ID from selection (Admin = 1, Employee = 2)
            int roleId = 1; // Default to Admin
            if (SelectedRole.Contains("Employee"))
            {
                roleId = 2;
            }
            else if (SelectedRole.Contains("Admin"))
            {
                roleId = 1;
            }

            var dto = new UserRegisterDto
            {
                FullName = FullName,
                Email = Email,
                Password = Password,
                PhoneNumber = "", // Optional field
                RoleId = roleId
            };

            _logger.LogInformation("[AddAdminViewModel] Attempting to create account: Email={Email}, Role={Role}", Email, roleId);

            // Use the enhanced method that returns error details
            var (response, errorMessage) = await _authClient.RegisterAsyncWithError(dto);

            if (response is not null)
            {
                StatusMessage = $"Account created successfully for {response.FullName}!";
                _logger.LogInformation("[AddAdminViewModel] Account created successfully: UserId={UserId}", response.Id);
                
                // Clear the form
                FullName = string.Empty;
                Email = string.Empty;
                Username = string.Empty;
                Password = string.Empty;
                SelectedRole = string.Empty;

                // Wait a moment to show success message, then navigate back
                await Task.Delay(1500);
                await Shell.Current.GoToAsync("///AdminAnalyticsPage");
            }
            else
            {
                // Use the actual error message from the API, or provide a generic one
                ErrorMessage = errorMessage ?? "Failed to create account. Please check the details and try again.";
                _logger.LogWarning("[AddAdminViewModel] Failed to create account: {Error}", errorMessage);
            }
        }
    }
}

