using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using System.Collections.ObjectModel;
using TRAFFIK_APP.Views;
using TRAFFIK_APP.Models.Dtos.Reward;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Models.Entities.Vehicle;
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

        public ObservableCollection<BookingDto> Bookings { get; } = new();
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
        public Command<RewardItemDto> RedeemCommand { get; }

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
            RedeemCommand = new Command<RewardItemDto>(async (item) => await RedeemReward(item));
        }

        public async Task LoadDashboardAsync()
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

            System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Starting to load dashboard data for user {userId}");
            
            var bookings = await _bookingClient.GetByUserAsync(userId);
            var notifications = await _notificationClient.GetAllAsync();
            var vehicleDtos = await _vehicleClient.GetByUserAsync(userId);
            System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] VehicleClient returned: {vehicleDtos?.Count ?? 0} vehicles");
            
            // Log the actual vehicle data
            if (vehicleDtos != null && vehicleDtos.Any())
            {
                foreach (var dto in vehicleDtos)
                {
                    System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Vehicle DTO: {dto.Make} {dto.Model} ({dto.LicensePlate})");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[DashboardViewModel] No vehicle DTOs returned from API");
            }
            
            var catalog = await _catalogClient.GetAllAsync();

            // Get reward balance
            var balance = await _rewardClient.GetBalanceAsync(userId);
            RewardBalance = balance ?? 0;

            if (bookings is not null)
                foreach (var b in bookings) Bookings.Add(b);

            if (notifications is not null)
                foreach (var n in notifications) Notifications.Add(n);
            
            // Notify UI that the Vehicles collection has changed
            OnPropertyChanged(nameof(Vehicles));

            if (vehicleDtos is not null)
            {
                System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Found {vehicleDtos.Count()} vehicles from API");
                var vehicles = vehicleDtos.Select(dto => new Vehicle
                {
                    VehicleType = dto.VehicleType,
                    Make = dto.Make,
                    Model = dto.Model,
                    LicensePlate = dto.LicensePlate,
                    ImageUrl = !string.IsNullOrEmpty(dto.ImageUrl) && !dto.ImageUrl.StartsWith("data:") 
                        ? $"data:image/jpeg;base64,{dto.ImageUrl}" 
                        : "dotnet_bot.png",
                    UserId = userId
                });

                foreach (var v in vehicles) 
                {
                    System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Adding vehicle: {v.Make} {v.Model} ({v.LicensePlate})");
                    Vehicles.Add(v);
                }
                System.Diagnostics.Debug.WriteLine($"[DashboardViewModel] Total vehicles in collection: {Vehicles.Count}");
                
                // Notify UI that the Vehicles collection has changed
                OnPropertyChanged(nameof(Vehicles));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[DashboardViewModel] No vehicles returned from API");
            }

            // Ensure we always have at least the test vehicle visible
            if (Vehicles.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("[DashboardViewModel] No vehicles found, adding fallback test vehicle");
                var fallbackVehicle = new Vehicle
                {
                    VehicleType = "Car",
                    Make = "Fallback",
                    Model = "Vehicle",
                    LicensePlate = "FALLBACK123",
                    ImageUrl = "dotnet_bot.png",
                    UserId = userId
                };
                Vehicles.Add(fallbackVehicle);
                OnPropertyChanged(nameof(Vehicles));
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

        private async Task RedeemReward(RewardItemDto item)
        {
            try
            {
                if (_session.UserId is not int userId)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please log in to redeem rewards.", "OK");
                    return;
                }

                int currentBalance = RewardBalance;
                if (currentBalance < item.Cost)
                {
                    await Application.Current.MainPage.DisplayAlert("Insufficient Points", 
                        $"You need {item.Cost} points to redeem this item. You currently have {currentBalance} points.", "OK");
                    return;
                }

                var confirmed = await Application.Current.MainPage.DisplayAlert("Confirm Redemption", 
                    $"Are you sure you want to redeem '{item.Name}' for {item.Cost} points?", "Yes", "No");

                if (confirmed)
                {
                    var response = await _catalogClient.RedeemItemAsync(item.Id, userId);

                    if (response?.Redeemed > 0)
                    {
                        // Display success message with the redemption code
                        var message = $"Successfully redeemed '{item.Name}'!\n\nYour redemption code: {response.Code}";
                        if (!string.IsNullOrEmpty(response.Code))
                        {
                            message = $"Successfully redeemed '{item.Name}'!\n\nYour redemption code:\n{response.Code}";
                        }
                        
                        await Application.Current.MainPage.DisplayAlert("Success", message, "OK");
                        
                        // Refresh the dashboard to update points and rewards
                        await LoadDashboardAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", 
                            "Failed to redeem item. Please try again.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", 
                    $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}