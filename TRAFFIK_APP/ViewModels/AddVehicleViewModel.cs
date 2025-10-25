using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Vehicle;

namespace TRAFFIK_APP.ViewModels
{
    public class AddVehicleViewModel : BaseViewModel
    {
        private readonly SessionService _session;
        private readonly VehicleClient _vehicleClient;
        private string _vehicleNickname = string.Empty;

        private string _vehicleMake = string.Empty;
        private string _vehicleModel = string.Empty;
        private string _licensePlate = string.Empty;
        private VehicleTypeDto _selectedVehicleType;
        private string _vehicleColor = string.Empty;
        private int _vehicleYear = DateTime.Now.Year;
        private ImageSource _vehicleImage = ImageSource.FromFile("vehicle_placeholder.png");
        public ObservableCollection<VehicleTypeDto> VehicleTypes { get; } = new();
        public VehicleTypeDto SelectedVehicleType { get; set; }

        public class VehicleTypeDto
        {
            public string Name { get; set; }
        }

        public byte[] VehicleImageBytes { get; private set; }
        public string UserFullName => _session.UserName;

        public string VehicleNickname
        {
            get => _vehicleNickname;
            set => SetProperty(ref _vehicleNickname, value);
        }
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

        public ImageSource VehicleImage
        {
            get => _vehicleImage;
            set => SetProperty(ref _vehicleImage, value);
        }

        public string VehicleColor
        {
            get => _vehicleColor;
            set => SetProperty(ref _vehicleColor, value);
        }

        public int VehicleYear
        {
            get => _vehicleYear;
            set => SetProperty(ref _vehicleYear, value);
        }

        public ICommand UploadImageCommand { get; }
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
            UploadImageCommand = new Command(async () => await PickImageAsync());

            _ = LoadVehicleTypesAsync();
        }

        private async Task LoadVehicleTypesAsync()
        {
            try
            {
                var types = await _vehicleClient.GetAllVehicleTypesAsync();
                VehicleTypes.Clear();

                if (types != null && types.Any())
                {
                    foreach (var type in types)
                        VehicleTypes.Add(new VehicleTypeDto { Name = type });
                }
                else
                {
                    ErrorMessage = "No vehicle types found.";
                }
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

                // Build a VehicleDto with exact property names for the backend
                var vehicleDto = new VehicleDto
                {
                    UserId = _session.UserId.Value,
                    Make = VehicleMake,
                    Model = VehicleModel,
                    LicensePlate = LicensePlate,
                    ImageUrl = VehicleImageBytes != null ? Convert.ToBase64String(VehicleImageBytes) : "",
                    VehicleType = SelectedVehicleType.Name,
                    Color = VehicleColor,
                    Year = VehicleYear
                };

                // Send DTO to backend
                var result = await _vehicleClient.CreateAsync(vehicleDto);

                if (result != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Vehicle added successfully!", "OK");

                    // Reset form
                    VehicleMake = string.Empty;
                    VehicleModel = string.Empty;
                    LicensePlate = string.Empty;
                    SelectedVehicleType = null;
                    VehicleColor = string.Empty;
                    VehicleYear = DateTime.Now.Year;
                    VehicleImage = ImageSource.FromFile("vehicle_placeholder.png");
                    VehicleImageBytes = null;

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

        private async Task PickImageAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select a vehicle image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    VehicleImageBytes = memoryStream.ToArray();

                    VehicleImage = ImageSource.FromStream(() => new MemoryStream(VehicleImageBytes));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Image pick error: {ex.Message}");
                ErrorMessage = "Failed to pick image.";
            }
        }
    }
}