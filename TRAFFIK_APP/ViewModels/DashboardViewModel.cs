using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using System.Collections.ObjectModel;
using TRAFFIK_APP.Views;

namespace TRAFFIK_APP.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly BookingClient _bookingClient;
        private readonly RewardClient _rewardClient;
        private readonly NotificationClient _notificationClient;
        private readonly VehicleClient _vehicleClient;
        private readonly SessionService _session;

        public ObservableCollection<Booking> Bookings { get; } = new();
        public ObservableCollection<Notification> Notifications { get; } = new();
        public ObservableCollection<Vehicle> Vehicles { get; } = new();

        public int RewardBalance { get; set; }
        public string UserFullName => _session.UserName;

        public Command LoadDashboardCommand { get; }
        public Command GoHomeCommand { get; }
        public Command GoAppointmentsCommand { get; }
        public Command GoRewardsCommand { get; }
        public Command GoAccountCommand { get; }

        public DashboardViewModel(
            BookingClient bookingClient,
            RewardClient rewardClient,
            NotificationClient notificationClient,
            VehicleClient vehicleClient,
            SessionService session)
        {
            _bookingClient = bookingClient;
            _rewardClient = rewardClient;
            _notificationClient = notificationClient;
            _vehicleClient = vehicleClient;
            _session = session;

            LoadDashboardCommand = new Command(() => ExecuteSafeAsync(LoadDashboardAsync, "Loading dashboard..."));
            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
        }

        private async Task LoadDashboardAsync()
        {
            if (_session.UserId is not int userId)
            {
                ErrorMessage = "Session expired. Please log in again.";
                await Shell.Current.GoToAsync(nameof(LoginPage));
                return;
            }

            Bookings.Clear();
            Notifications.Clear();
            Vehicles.Clear();

            var bookings = await _bookingClient.GetByUserAsync(userId);
            var notifications = await _notificationClient.GetAllAsync();
            var balance = await _rewardClient.GetBalanceAsync(userId);
            var vehicleDtos = await _vehicleClient.GetByUserAsync(userId);

            if (bookings is not null)
                foreach (var b in bookings) Bookings.Add(b);

            if (notifications is not null)
                foreach (var n in notifications) Notifications.Add(n);

            RewardBalance = balance ?? 0;

            if (vehicleDtos is not null)
            {
                var vehicles = vehicleDtos.Select(dto => new Vehicle
                {
                    Type = dto.VehicleType,
                    Make = dto.Make,
                    Model = dto.Model,
                    LicensePlate = dto.LicensePlate,
                    ImageUrl = dto.ImageUrl,
                    UserId = userId
                });

                foreach (var v in vehicles) Vehicles.Add(v);
            }
        }
    }
}
