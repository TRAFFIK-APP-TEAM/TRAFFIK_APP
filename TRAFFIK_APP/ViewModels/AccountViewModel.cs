using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Views;
using System.Collections.ObjectModel;
using TRAFFIK_APP.Models.Dtos.User;

namespace TRAFFIK_APP.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        private readonly SessionService _session;
        private readonly VehicleClient _vehicleClient;
        private readonly UserClient _userClient;
        
        private string _name = string.Empty;
        private string _surname = string.Empty;
        private string _email = string.Empty;
        private string _phoneNumber = string.Empty;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                SetProperty(ref _surname, value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string FullName => $"{Name} {Surname}".Trim();

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public ObservableCollection<VehicleItem> Vehicles { get; } = new();

        public Command LogoutCommand { get; }
        public Command SaveProfileCommand { get; }
        public Command AddVehicleCommand { get; }
        public Command GoHomeCommand { get; }
        public Command GoAppointmentsCommand { get; }
        public Command GoRewardsCommand { get; }
        public Command GoAccountCommand { get; }

        public AccountViewModel(SessionService session, VehicleClient vehicleClient, UserClient userClient)
        {
            _session = session;
            _vehicleClient = vehicleClient;
            _userClient = userClient;
            
            LogoutCommand = new Command(() => ExecuteSafeAsync(LogoutAsync, "Logging out..."));
            SaveProfileCommand = new Command(() => ExecuteSafeAsync(SaveProfileAsync, "Saving profile..."));
            AddVehicleCommand = new Command(() => ExecuteSafeAsync(AddVehicleAsync, "Loading..."));
            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
            
            // Load user info
            LoadUserInfo();
            
            // Load vehicles
            _ = LoadVehiclesAsync();
        }

        private void LoadUserInfo()
        {
            if (_session.CurrentUser != null)
            {
                var fullNameParts = _session.CurrentUser.FullName.Split(' ', 2);
                Name = fullNameParts.Length > 0 ? fullNameParts[0] : string.Empty;
                Surname = fullNameParts.Length > 1 ? fullNameParts[1] : string.Empty;
                Email = _session.CurrentUser.Email;
                PhoneNumber = _session.CurrentUser.PhoneNumber ?? string.Empty;
            }
        }

        private async Task LoadVehiclesAsync()
        {
            try
            {
                if (_session.UserId.HasValue)
                {
                    var vehicles = await _vehicleClient.GetByUserAsync(_session.UserId.Value);
                    
                    Vehicles.Clear();
                    if (vehicles != null && vehicles.Count > 0)
                    {
                        foreach (var vehicle in vehicles)
                        {
                            Vehicles.Add(new VehicleItem
                            {
                                LicensePlate = vehicle.LicensePlate,
                                Make = vehicle.Make ?? "Unknown",
                                Model = vehicle.Model ?? "Unknown",
                                ImageUrl = !string.IsNullOrEmpty(vehicle.ImageUrl) ? vehicle.ImageUrl : "car_placeholder.png"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading vehicles: {ex.Message}");
            }
        }

        private async Task SaveProfileAsync()
        {
            try
            {
                if (_session.CurrentUser != null)
                {
                    // Create user update DTO
                    var userUpdateDto = new UserUpdateDto
                    {
                        Id = _session.CurrentUser.Id,
                        FullName = FullName,
                        Email = Email,
                        PhoneNumber = PhoneNumber,
                        RoleId = _session.CurrentUser.RoleId
                    };

                    // Call API to update user profile
                    var success = await _userClient.UpdateAsync(_session.CurrentUser.Id, _session.CurrentUser);
                    
                    if (success)
                    {
                        // Update session and secure storage
                        _session.CurrentUser.FullName = FullName;
                        _session.CurrentUser.Email = Email;
                        _session.CurrentUser.PhoneNumber = PhoneNumber;
                        
                        await SecureStorage.SetAsync("user_name", FullName);
                        await SecureStorage.SetAsync("email", Email);
                        
                        await Application.Current.MainPage.DisplayAlert("Success", "Profile updated successfully!", "OK");
                    }
                    else
                    {
                        ErrorMessage = "Failed to update profile. Please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error saving profile: {ex.Message}";
            }
        }

        private async Task AddVehicleAsync()
        {
            try
            {
                // Navigate to Add Vehicle Page
                await Shell.Current.GoToAsync(nameof(AddVehiclePage));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        private async Task LogoutAsync()
        {
            try
            {
                // Clear session service
                _session.Clear();

                // Clear secure storage
                SecureStorage.Remove("user_id");
                SecureStorage.Remove("user_name");
                SecureStorage.Remove("role_id");
                SecureStorage.Remove("email");

                // Switch back to login shell
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during logout: {ex.Message}";
            }
        }
    }

    // Vehicle display
    public class VehicleItem
    {
        public string LicensePlate { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}


