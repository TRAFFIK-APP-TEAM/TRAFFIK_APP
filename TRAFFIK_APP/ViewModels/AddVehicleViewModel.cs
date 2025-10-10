using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TRAFFIK_APP.ViewModels
{
    public class AddVehicleViewModel : BaseViewModel
    {
        private readonly SessionService _session;
        private readonly VehicleClient _vehicleClient;

        //private string _vehicleNickname = string.Empty;
        private string _vehicleMake = string.Empty;
        private string _vehicleModel = string.Empty;
        private string _licensePlate = string.Empty;
        private string _selectedVehicleType = string.Empty;

        public string VehicleMake
        {
            get => _vehicleMake;
            set => SetProperty(ref _vehicleMake, value);
        }

        public string VehicleModel
        {
            get => _vehicleModel;
            set => SetProperty(ref _vehicleModel, value);
        }

        public string LicensePlate
        {
            get => _licensePlate;
            set => SetProperty(ref _licensePlate, value);
        }

        public string SelectedVehicleType
        {
            get => _selectedVehicleType;
            set => SetProperty(ref _selectedVehicleType, value);
        }

        public ObservableCollection<Vehicle> VehicleTypes { get; } = new();

        public string UserFullName => _session.UserName;

        public ICommand AddVehicleCommand { get; }
        public ICommand GoHomeCommand { get; }
        public ICommand GoAppointmentsCommand { get; }
        public ICommand GoRewardsCommand { get; }
        public ICommand GoAccountCommand { get; }

        public AddVehicleViewModel(SessionService session, VehicleClient vehicleClient)
        {
            _session = session;
            _vehicleClient = vehicleClient;

            AddVehicleCommand = new Command(async () => await ExecuteSafeAsync(AddVehicleAsync, "Saving vehicle..."));
            GoHomeCommand = new Command(async () => await NavigateToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await NavigateToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await NavigateToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await NavigateToAsync("//AccountPage"));

            // Load vehicle types
            _ = LoadVehicleTypesAsync();
        }

        private async Task LoadVehicleTypesAsync()
        {
            try
            {
                var types = await _vehicleClient.GetAllVehicleTypesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading vehicle types: {ex.Message}");
                ErrorMessage = "Failed to load vehicle types.";
            }
        }

        private async Task AddVehicleAsync()
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(VehicleMake))
                {
                    ErrorMessage = "Please enter the vehicle make.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(VehicleModel))
                {
                    ErrorMessage = "Please enter the vehicle model.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(LicensePlate))
                {
                    ErrorMessage = "Please enter the vehicle License Plate.";
                    return;
                }

                if (SelectedVehicleType == null)
                {
                    ErrorMessage = "Please select a vehicle type.";
                    return;
                }

                if (!_session.UserId.HasValue)
                {
                    ErrorMessage = "User session expired. Please log in again.";
                    return;
                }

                // Create vehicle DTO
                // Note: VehicleDto doesn't have all properties, so we'll use what's available
                // The nickname and color can be stored in the Model field or we may need to update the DTO
                //removed VehicleNickname and colour, can reintroduce once table corrected
                var vehicleDto = new VehicleDto
                {
                    Make = VehicleMake,
                    Model = VehicleModel,
                    LicensePlate = LicensePlate,
                    ImageUrl = "", // Default empty for now
                    VehicleType = SelectedVehicleType // This is the string selected from the dropdown
                };

                var result = await _vehicleClient.CreateAsync(vehicleDto);

                if (result != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Vehicle added successfully!", "OK");
                    
                    // Navigate back to Account page
                    await Shell.Current.GoToAsync("//AccountPage");
                }
                else
                {
                    ErrorMessage = "Failed to add vehicle. Please try again.";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding vehicle: {ex.Message}");
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        private async Task NavigateToAsync(string route)
        {
            try
            {
                await Shell.Current.GoToAsync(route);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
            }
        }
    }
}

