using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using System.Collections.ObjectModel;
using TRAFFIK_APP.Views;
using TRAFFIK_APP.Models.Dtos.Reward;
using TRAFFIK_APP.Models.Entities.Vehicle;
using TRAFFIK_APP.Models.Entities.Booking;
using TRAFFIK_APP.Models.Entities.Notification;

namespace TRAFFIK_APP.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly BookingClient _bookingClient;
        private readonly RewardClient _rewardClient;
        private readonly RewardCatalogClient _catalogClient;
        private readonly NotificationClient _notificationClient;
        private readonly VehicleClient _vehicleClient;
        private readonly SessionService _session;

        public ObservableCollection<Booking> Bookings { get; } = new();
        public ObservableCollection<Notification> Notifications { get; } = new();
        public ObservableCollection<Vehicle> Vehicles { get; } = new();
        public ObservableCollection<RewardItemDto> AvailableRewards { get; } = new();
        public ObservableCollection<RewardItemDto> LockedRewards { get; } = new();

        public IEnumerable<RewardItemDto> TopAvailableRewards => AvailableRewards.Take(3);
        public IEnumerable<RewardItemDto> FourthLockedReward => LockedRewards.Take(1);

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
            RewardCatalogClient catalogClient,
            NotificationClient notificationClient,
            VehicleClient vehicleClient,
            SessionService session)
        {
            _bookingClient = bookingClient;
            _rewardClient = rewardClient;
            _catalogClient = catalogClient;
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
            AvailableRewards.Clear();
            LockedRewards.Clear();

            var bookings = await _bookingClient.GetByUserAsync(userId);
            var notifications = await _notificationClient.GetAllAsync();
            var balance = await _rewardClient.GetBalanceAsync(userId);
            var vehicleDtos = await _vehicleClient.GetByUserAsync(userId);
            var catalog = await _catalogClient.GetAllAsync();

            RewardBalance = balance ?? 0;

            if (bookings is not null)
                foreach (var b in bookings) Bookings.Add(b);

            if (notifications is not null)
                foreach (var n in notifications) Notifications.Add(n);

            if (vehicleDtos is not null)
            {
                var vehicles = vehicleDtos.Select(dto => new Vehicle
                {
                    VehicleType = dto.VehicleType,
                    Make = dto.Make,
                    Model = dto.Model,
                    LicensePlate = dto.LicensePlate,
                    ImageUrl = !string.IsNullOrEmpty(dto.ImageUrl) && !dto.ImageUrl.StartsWith("data:") 
                        ? $"data:image/jpeg;base64,{dto.ImageUrl}" 
                        : "car_placeholder.png",
                    UserId = userId
                });

                foreach (var v in vehicles) Vehicles.Add(v);
            }

            if (catalog is not null)
            {
                foreach (var reward in catalog)
                {
                    if (reward.Cost <= RewardBalance)
                        AvailableRewards.Add(reward);
                    else
                        LockedRewards.Add(reward);
                }
            }

            OnPropertyChanged(nameof(RewardBalance));
            OnPropertyChanged(nameof(TopAvailableRewards));
            OnPropertyChanged(nameof(FourthLockedReward));
        }
    }
}