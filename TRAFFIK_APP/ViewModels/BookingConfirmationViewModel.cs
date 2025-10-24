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
            if (SelectedVehicle == null || SelectedService == null || _sessionService.UserId is not int userId)
            {
                await Shell.Current.DisplayAlert("Error", "Missing booking information. Please try again.", "OK");
                return;
            }

            try
            {
                // Create booking entity (not DTO)
                var booking = new Booking
                {
                    UserId = userId,
                    LicensePlate = SelectedVehicle.LicensePlate,
                    ServiceCatalogId = SelectedService.Id,
                    ScheduledDate = SelectedDateTime,
                    Location = "TRAFFIK Service Center", // Default location
                    IsConfirmed = true,
                    Status = "Confirmed",
                    CreatedAt = DateTime.Now
                };

                // Create the booking
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
                System.Diagnostics.Debug.WriteLine($"Error creating booking: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Failed to create booking. Please try again.", "OK");
            }
        }
    }
}
