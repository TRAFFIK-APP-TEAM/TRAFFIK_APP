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

        public BookingConfirmationViewModel(BookingClient bookingClient, RewardClient rewardClient, SessionService sessionService)
        {
            _bookingClient = bookingClient;
            _rewardClient = rewardClient;
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
                // Create booking entity that matches the API model
                // Note: CarModelId is no longer used as the API uses license plate directly
                var booking = new Booking
                {
                    UserId = userId,
                    ServiceCatalogId = SelectedService.Id,
                    VehicleLicensePlate = SelectedVehicle.LicensePlate,
                    BookingDate = DateOnly.FromDateTime(SelectedDateTime),
                    BookingTime = TimeOnly.FromDateTime(SelectedDateTime),
                    Status = "Pending" // Start as Pending, will be confirmed by API
                };

                // Debug: Log the booking data being sent
                System.Diagnostics.Debug.WriteLine($"[BookingConfirmation] Creating booking with data:");
                System.Diagnostics.Debug.WriteLine($"  UserId: {booking.UserId}");
                System.Diagnostics.Debug.WriteLine($"  ServiceCatalogId: {booking.ServiceCatalogId}");
                System.Diagnostics.Debug.WriteLine($"  VehicleLicensePlate: {booking.VehicleLicensePlate}");
                System.Diagnostics.Debug.WriteLine($"  BookingDate: {booking.BookingDate}");
                System.Diagnostics.Debug.WriteLine($"  BookingTime: {booking.BookingTime}");
                System.Diagnostics.Debug.WriteLine($"  Status: {booking.Status}");

                // Create the booking using the entity
                var createdBooking = await _bookingClient.CreateAsync(booking);
                
                if (createdBooking != null)
                {
                    // Note: Reward points are automatically awarded by the backend
                    // No need to manually call EarnAsync

                    // Reset the booking flow state to fresh
                    ResetBookingState();

                    await Shell.Current.DisplayAlert("Success!", 
                        $"Your booking has been confirmed!", "OK");
                    
                    // Navigate back to bookings page and reload bookings
                    await Shell.Current.GoToAsync("//BookingPage");
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

        /// <summary>
        /// Resets all booking selection state to prepare for a new booking flow
        /// </summary>
        public static void ResetBookingState()
        {
            // Reset BookingConfirmationViewModel state
            SelectedService = null;
            SelectedVehicle = null;
            SelectedDateTime = default(DateTime);
            
            // Reset BookingDateTimeSelectViewModel state
            BookingDateTimeSelectViewModel.SelectedService = null;
            BookingDateTimeSelectViewModel.SelectedVehicle = null;
            
            // Reset BookingServiceSelectViewModel state
            BookingServiceSelectViewModel.SelectedVehicle = null;
        }
    }
}
