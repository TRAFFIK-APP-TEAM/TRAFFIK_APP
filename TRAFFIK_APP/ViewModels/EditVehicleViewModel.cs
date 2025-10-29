using TRAFFIK_APP.Models.Entities.Vehicle;
using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TRAFFIK_APP.ViewModels
{
    public class EditVehicleViewModel : BaseViewModel
    {
        private readonly VehicleClient _vehicleClient;
        private readonly SessionService _session;
        private Vehicle? _originalVehicle;

        // Properties for form binding
        private string _licensePlate = string.Empty;
        private string _make = string.Empty;
        private string _model = string.Empty;
        private string _vehicleType = string.Empty;
        private string _color = string.Empty;
        private int _year = DateTime.Now.Year;
        private ImageSource _vehicleImage = ImageSource.FromFile("dotnet_bot.png");
        private string _selectedVehicleType = string.Empty;

        public ObservableCollection<string> VehicleTypes { get; } = new();

        public string LicensePlate
        {
            get => _licensePlate;
            set => SetProperty(ref _licensePlate, value);
        }

        public string Make
        {
            get => _make;
            set => SetProperty(ref _make, value);
        }

        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public string VehicleType
        {
            get => _vehicleType;
            set => SetProperty(ref _vehicleType, value);
        }

        public string SelectedVehicleType
        {
            get => _selectedVehicleType;
            set => SetProperty(ref _selectedVehicleType, value);
        }

        public string Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        public ImageSource VehicleImage
        {
            get => _vehicleImage;
            set => SetProperty(ref _vehicleImage, value);
        }

        // Commands
        public Command GoBackCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command ChangeImageCommand { get; }

        public EditVehicleViewModel(VehicleClient vehicleClient, SessionService session)
        {
            _vehicleClient = vehicleClient;
            _session = session;

            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            SaveCommand = new Command(async () => await SaveVehicleAsync());
            DeleteCommand = new Command(async () => await DeleteVehicleAsync());
            ChangeImageCommand = new Command(async () => await ChangeImageAsync());

            LoadVehicleTypesAsync();
        }

        public void SetVehicle(Vehicle vehicle)
        {
            _originalVehicle = vehicle;
            LicensePlate = vehicle.LicensePlate;
            Make = vehicle.Make;
            Model = vehicle.Model;
            VehicleType = vehicle.VehicleType;
            SelectedVehicleType = vehicle.VehicleType;
            Color = vehicle.Color;
            Year = vehicle.Year;
            VehicleImage = !string.IsNullOrEmpty(vehicle.ImageUrl) 
                ? ImageSource.FromUri(new Uri(vehicle.ImageUrl)) 
                : ImageSource.FromFile("dotnet_bot.png");
        }

        private async Task LoadVehicleTypesAsync()
        {
            try
            {
                var types = await _vehicleClient.GetAllVehicleTypesAsync();
                if (types != null)
                {
                    VehicleTypes.Clear();
                    foreach (var type in types)
                    {
                        VehicleTypes.Add(type.Type);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EditVehicleViewModel] Error loading vehicle types: {ex.Message}");
            }
        }

        private async Task SaveVehicleAsync()
        {
            if (string.IsNullOrWhiteSpace(Make) || string.IsNullOrWhiteSpace(Model) || 
                string.IsNullOrWhiteSpace(SelectedVehicleType) || Year <= 1900)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all required fields.", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                var vehicleDto = new Models.Dtos.Vehicle.VehicleDto
                {
                    LicensePlate = LicensePlate,
                    UserId = (int)_session.UserId,
                    Make = Make,
                    Model = Model,
                    VehicleType = SelectedVehicleType,
                    Color = Color,
                    Year = Year,
                    ImageUrl = _originalVehicle?.ImageUrl ?? string.Empty
                };

                var success = await _vehicleClient.UpdateAsync(LicensePlate, vehicleDto);
                
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Vehicle updated successfully!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update vehicle. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EditVehicleViewModel] Error saving vehicle: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while updating the vehicle.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteVehicleAsync()
        {
            var result = await Application.Current.MainPage.DisplayAlert(
                "Delete Vehicle", 
                "Are you sure you want to delete this vehicle? This action cannot be undone.", 
                "Delete", 
                "Cancel");

            if (!result) return;

            try
            {
                IsBusy = true;

                var success = await _vehicleClient.DeleteAsync(LicensePlate);
                
                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Vehicle deleted successfully!", "OK");
                    await Shell.Current.GoToAsync("//DashboardPage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete vehicle. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EditVehicleViewModel] Error deleting vehicle: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while deleting the vehicle.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ChangeImageAsync()
        {
            try
            {
                var result = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Vehicle Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    VehicleImage = ImageSource.FromFile(result.FullPath);
                    // You could also convert to base64 and store in a property for API submission
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EditVehicleViewModel] Error picking image: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to select image.", "OK");
            }
        }
    }
}
