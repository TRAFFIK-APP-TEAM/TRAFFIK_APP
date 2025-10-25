using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Models.Dtos.Vehicle;
using TRAFFIK_APP.Models.Dtos.ServiceCatalog;
using TRAFFIK_APP.Models.Dtos.Reward;
using TRAFFIK_APP.Models.Entities.Booking;
using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingConfirmationViewModel : BaseViewModel
    {
        private readonly BookingClient _bookingClient;
        private readonly RewardClient _rewardClient;
        private readonly CarModelClient _carModelClient;
        private readonly SessionService _sessionService;

        // Static properties to hold selected data between pages
        public static ServiceCatalogItem SelectedService { get; set; }
        public static VehicleDto SelectedVehicle { get; set; }
        public static DateTime SelectedDateTime { get; set; }

        public string SelectedDate => SelectedDateTime.ToString("dddd, dd MMM yyyy");
        public string SelectedTime => SelectedDateTime.ToString("HH:mm");
        
        // Properties for UI binding
        public string SelectedServiceName => SelectedService?.Name ?? "No Service Selected";
        public string SelectedVehicleName => SelectedVehicle?.DisplayName ?? "No Vehicle Selected";
        public string SelectedServicePrice => SelectedService?.Price.ToString("C") ?? "N/A";
        public string SelectedServiceDescription => SelectedService?.Description ?? "No description available";
        
        // Additional properties for remaining bindings
        public string SelectedVehicleType => SelectedVehicle?.VehicleType ?? "Unknown";
        public string SelectedVehicleLicensePlate => SelectedVehicle?.LicensePlate ?? "Unknown";

        public ICommand ConfirmBookingCommand { get; }
        public ICommand GoBackCommand { get; }

        public BookingConfirmationViewModel(BookingClient bookingClient, RewardClient rewardClient, CarModelClient carModelClient, SessionService sessionService)
        {
            _bookingClient = bookingClient;
            _rewardClient = rewardClient;
            _carModelClient = carModelClient;
            _sessionService = sessionService;

            ConfirmBookingCommand = new Command(() => ExecuteSafeAsync(OnConfirmBooking, "Confirming booking..."));
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }


        private async Task OnConfirmBooking()
        {
            System.Diagnostics.Debug.WriteLine("[BookingConfirmation] OnConfirmBooking method called");
            
            if (SelectedVehicle == null || SelectedService == null || _sessionService.UserId is not int userId)
            {
                System.Diagnostics.Debug.WriteLine("[BookingConfirmation] Missing booking information - showing alert");
                await Shell.Current.DisplayAlert("Error", "Missing booking information. Please try again.", "OK");
                return;
            }
            
            System.Diagnostics.Debug.WriteLine($"[BookingConfirmation] All required data present - proceeding with booking creation");

            try
            {
                // First, create or get a CarModel for this user and vehicle
                int carModelId;
                try
                {
                    carModelId = await _carModelClient.CreateOrGetAsync(
                        userId,
                        SelectedVehicle.VehicleType,
                        SelectedVehicle.Make,
                        SelectedVehicle.Model,
                        SelectedVehicle.LicensePlate,
                        SelectedVehicle.Year
                    );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[BookingConfirmation] CarModel creation failed: {ex.Message}");
                    // Use a default CarModelId for now (this is a temporary workaround)
                    // In a real scenario, you'd want to create the CarModel first
                    carModelId = 1; // This should be replaced with actual CarModel creation
                    System.Diagnostics.Debug.WriteLine($"[BookingConfirmation] Using default CarModelId: {carModelId}");
                }

                // Create booking entity that matches the API model
                // Now includes VehicleLicensePlate field as required by API
                var booking = new Booking
                {
                    UserId = userId,
                    ServiceId = 0, // Legacy field, set to 0 instead of null
                    CarModelId = carModelId, // Use the created/retrieved CarModelId
                    ServiceCatalogId = SelectedService.Id, // Use ServiceCatalogId as the primary service reference
                    VehicleLicensePlate = SelectedVehicle.LicensePlate, // Add the required VehicleLicensePlate field
                    BookingDate = DateOnly.FromDateTime(SelectedDateTime),
                    BookingTime = TimeOnly.FromDateTime(SelectedDateTime),
                    Status = "Pending" // Start as Pending, will be confirmed by API
                };

                // Debug: Log the booking data being sent
                System.Diagnostics.Debug.WriteLine($"[BookingConfirmation] Creating booking with data:");
                System.Diagnostics.Debug.WriteLine($"  UserId: {booking.UserId}");
                System.Diagnostics.Debug.WriteLine($"  ServiceId: {booking.ServiceId} (legacy, not used)");
                System.Diagnostics.Debug.WriteLine($"  CarModelId: {booking.CarModelId}");
                System.Diagnostics.Debug.WriteLine($"  ServiceCatalogId: {booking.ServiceCatalogId} (primary service reference)");
                System.Diagnostics.Debug.WriteLine($"  VehicleLicensePlate: {booking.VehicleLicensePlate}");
                System.Diagnostics.Debug.WriteLine($"  BookingDate: {booking.BookingDate}");
                System.Diagnostics.Debug.WriteLine($"  BookingTime: {booking.BookingTime}");
                System.Diagnostics.Debug.WriteLine($"  Status: {booking.Status}");

                // Create the booking using the entity (BookingClient handles the wrapper internally)
                var createdBooking = await _bookingClient.CreateAsync(booking);
                
                if (createdBooking != null)
                {
                    // Award reward points using EarnRewardRequest
                    var pointsToAward = (int)(SelectedService.Price / 10);
                    if (pointsToAward > 0)
                    {
                        var earnRequest = new EarnRewardRequest
                        {
                            UserId = userId,
                            BookingId = createdBooking.Id,
                            AmountSpent = SelectedService.Price
                        };
                        await _rewardClient.EarnAsync(earnRequest);
                    }

                    await Shell.Current.DisplayAlert("Success!", 
                        $"Your booking has been confirmed!\n\nYou earned {pointsToAward} reward points.", "OK");
                    
                    // Navigate back to dashboard
                    await Shell.Current.GoToAsync("//DashboardPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to create booking. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BookingConfirmation] Error creating booking: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[BookingConfirmation] Stack trace: {ex.StackTrace}");
                await Shell.Current.DisplayAlert("Error", $"Failed to create booking: {ex.Message}", "OK");
            }
        }
    }
}
