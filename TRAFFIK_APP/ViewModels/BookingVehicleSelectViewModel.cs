using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TRAFFIK_APP.Models.Dtos.Vehicle;
using TRAFFIK_APP.Views;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingVehicleSelectViewModel : BaseViewModel
    {
        private readonly VehicleClient _vehicleClient;
        private readonly SessionService _session;

        // Vehicle list
        public ObservableCollection<VehicleDto> Vehicles { get; } = new();

        // Properties for UI binding
        public bool HasVehicles => Vehicles.Count > 0;
        public bool HasNoVehicles => Vehicles.Count == 0;

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        // Commands
        public Command<VehicleDto> SelectVehicleCommand { get; }
        public Command GoHomeCommand { get; }
        public Command GoAppointmentsCommand { get; }
        public Command GoRewardsCommand { get; }
        public Command GoAccountCommand { get; }
        public Command LoadVehiclesCommand { get; }
        public Command AddVehicleCommand { get; }

        public BookingVehicleSelectViewModel(VehicleClient vehicleClient, SessionService session)
        {
            _vehicleClient = vehicleClient;
            _session = session;

            // Commands
                    SelectVehicleCommand = new Command<VehicleDto>(async vehicle =>
                    {
                        if (vehicle == null)
                            return;

                        System.Diagnostics.Debug.WriteLine($"Selected vehicle: {vehicle.DisplayName} (Type: {vehicle.VehicleType})");

                        // Store the selected vehicle in a static property for the next page
                        BookingServiceSelectViewModel.SelectedVehicle = vehicle;

                        // Navigate to service selection
                        await Shell.Current.GoToAsync(nameof(TRAFFIK_APP.Views.BookingServiceSelectPage));
                    });

            AddVehicleCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(TRAFFIK_APP.Views.AddVehiclePage)));

            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
            LoadVehiclesCommand = new Command(() => ExecuteSafeAsync(LoadVehiclesAsync, "Loading vehicles..."));

            // Load vehicles on initialization
            _ = LoadVehiclesAsync();
        }

        private async Task LoadVehiclesAsync()
        {
            try
            {
                IsRefreshing = true;
                
                if (_session.UserId is not int userId)
                {
                    ErrorMessage = "Session expired. Please log in again.";
                    return;
                }

                var vehicleDtos = await _vehicleClient.GetByUserAsync(userId);
                Vehicles.Clear();

                if (vehicleDtos != null)
                {
                    foreach (var vehicle in vehicleDtos)
                    {
                        Vehicles.Add(vehicle);
                    }
                }

                // Notify UI of changes
                OnPropertyChanged(nameof(HasVehicles));
                OnPropertyChanged(nameof(HasNoVehicles));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading vehicles: {ex.Message}");
                ErrorMessage = "Failed to load vehicles. Please try again.";
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
