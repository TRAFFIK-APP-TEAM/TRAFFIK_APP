using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Models.Entities;
using TRAFFIK_APP.Models.Entities.Notification;
using TRAFFIK_APP.Models.Entities.Reward;
using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;

namespace TRAFFIK_APP.ViewModels
{
    public class StaffBookingDetailViewModel : BaseViewModel
    {
        private readonly BookingStagesClient _bookingStagesClient;
        private readonly NotificationClient _notificationClient;
        private readonly RewardClient _rewardClient;
        private readonly BookingClient _bookingClient;
        private readonly ServiceCatalogClient _serviceCatalogClient;
        private readonly UserClient _userClient;

        public BookingStageUpdateDto? CurrentBooking { get; set; }
        public BookingDto? BookingDetails { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string ServiceName { get; private set; } = string.Empty;

        public ICommand UpdateStageCommand { get; }
        public ICommand GoBackCommand { get; }

        public StaffBookingDetailViewModel(BookingStagesClient bookingStagesClient, NotificationClient notificationClient, RewardClient rewardClient, BookingClient bookingClient, ServiceCatalogClient serviceCatalogClient, UserClient userClient)
        {
            _bookingStagesClient = bookingStagesClient;
            _notificationClient = notificationClient;
            _rewardClient = rewardClient;
            _bookingClient = bookingClient;
            _serviceCatalogClient = serviceCatalogClient;
            _userClient = userClient;

            UpdateStageCommand = new Command(() => ExecuteSafeAsync(UpdateStageAsync, "Updating status..."));
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public async Task LoadBookingDetailsAsync(int bookingId)
        {
            IsBusy = true;
            try
            {
                // Load the booking details
                var booking = await _bookingClient.GetByIdAsync(bookingId);
                if (booking != null)
                {
                    BookingDetails = new BookingDto
                    {
                        Id = booking.Id,
                        UserId = booking.UserId,
                        ServiceCatalogId = booking.ServiceCatalogId,
                        VehicleLicensePlate = booking.VehicleLicensePlate,
                        BookingDate = booking.BookingDate,
                        BookingTime = booking.BookingTime,
                        Status = booking.Status
                    };

                    // Load user details
                    var user = await _userClient.GetByIdAsync(booking.UserId);
                    if (user != null)
                    {
                        UserName = user.FullName;
                    }

                    // Load service details
                    if (booking.ServiceCatalogId.HasValue)
                    {
                        var service = await _serviceCatalogClient.GetByIdAsync(booking.ServiceCatalogId.Value);
                        if (service != null)
                        {
                            ServiceName = service.Name;
                        }
                    }
                }

                // Load the booking stage
                var stage = await _bookingStagesClient.GetByBookingAsync(bookingId);
                if (stage != null && stage.Count > 0)
                {
                    CurrentBooking = stage[0];
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdateStageAsync()
        {
            if (CurrentBooking is null || string.IsNullOrEmpty(CurrentBooking.SelectedStage)) return;

            // Fetch booking details to get UserId
            var bookingDetails = await _bookingClient.GetByIdAsync(CurrentBooking.BookingId);
            if (bookingDetails is null)
            {
                ErrorMessage = "Failed to fetch booking details.";
                return;
            }

            // Update the stage
            var success = await _bookingStagesClient.UpdateStageAsync(CurrentBooking);
            if (!success)
            {
                ErrorMessage = "Failed to update status.";
                return;
            }

            // Handle Service Completed stage - send notification to user
            if (CurrentBooking.SelectedStage == "Service Completed")
            {
                var notification = new Notification
                {
                    UserId = bookingDetails.UserId,
                    Title = "Service Completed",
                    Message = $"Your service for booking #{CurrentBooking.BookingId} has been completed. Please proceed with payment.",
                    Timestamp = DateTime.Now,
                    Type = "Info",
                    IsRead = false
                };

                await _notificationClient.CreateAsync(notification);
            }

            // Handle Service Paid stage - award points to user
            if (CurrentBooking.SelectedStage == "Service Paid")
            {
                // Fetch service details to get the price
                decimal pointsToAward = 0;
                string serviceName = "Service";

                if (bookingDetails.ServiceCatalogId.HasValue)
                {
                    var service = await _serviceCatalogClient.GetByIdAsync(bookingDetails.ServiceCatalogId.Value);
                    if (service != null)
                    {
                        // Calculate points: R10 = 1 point
                        pointsToAward = Math.Floor(service.Price / 10);
                        serviceName = service.Name;
                    }
                }

                // Award points based on service price
                var reward = new Reward
                {
                    UserId = bookingDetails.UserId,
                    Points = (int)pointsToAward,
                    Description = $"Points awarded for completed booking #{CurrentBooking.BookingId} - {serviceName}",
                    EarnedAt = DateTime.Now,
                    IsRedeemed = false
                };

                await _rewardClient.CreateAsync(reward);

                // Send notification about points
                var pointsNotification = new Notification
                {
                    UserId = bookingDetails.UserId,
                    Title = "Points Awarded!",
                    Message = $"You've earned {(int)pointsToAward} points for completing your booking #{CurrentBooking.BookingId}.",
                    Timestamp = DateTime.Now,
                    Type = "Success",
                    IsRead = false
                };

                await _notificationClient.CreateAsync(pointsNotification);
            }

            // Reload booking details
            await LoadBookingDetailsAsync(CurrentBooking.BookingId);
        }
    }
}

