using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TRAFFIK_APP.Models.Dtos.Booking;
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
        public ObservableCollection<BookingVehicleDto> Vehicles { get; } = new();

        // Commands
        public Command<BookingVehicleDto> SelectVehicleCommand { get; }
        public Command GoHomeCommand { get; }
        public Command GoAppointmentsCommand { get; }
        public Command GoRewardsCommand { get; }
        public Command GoAccountCommand { get; }
        public Command LoadVehiclesCommand { get; }

        public BookingVehicleSelectViewModel(VehicleClient vehicleClient, SessionService session)
        {
            _vehicleClient = vehicleClient;
            _session = session;

            // Commands
            SelectVehicleCommand = new Command<BookingVehicleDto>(async vehicle =>
            {
                if (vehicle == null)
                    return;

                await Shell.Current.DisplayAlert("Vehicle Selected",
                    $"You chose {vehicle.VehicleDisplayName}", "OK");
            });

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
                        Vehicles.Add(new BookingVehicleDto
                        {
                            Id = vehicle.Id,
                            UserId = vehicle.UserId,
                            VehicleDisplayName = $"{vehicle.Make} {vehicle.Model} ({vehicle.LicensePlate})",
                            Make = vehicle.Make,
                            Model = vehicle.Model,
                            LicensePlate = vehicle.LicensePlate,
                            VehicleType = vehicle.VehicleType
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading vehicles: {ex.Message}");
                ErrorMessage = "Failed to load vehicles. Please try again.";
            }
        }
    }
}
