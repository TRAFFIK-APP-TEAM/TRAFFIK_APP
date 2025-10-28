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

        private BookingStageUpdateDto? _currentBooking;
        public BookingStageUpdateDto? CurrentBooking 
        { 
            get => _currentBooking;
            set 
            { 
                SetProperty(ref _currentBooking, value);
                
                // Explicitly notify that nested properties may have changed
                if (value != null)
                {
                    OnPropertyChanged(nameof(CurrentBooking));
                }
                
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(Stage2Color));
                OnPropertyChanged(nameof(Stage3Color));
                OnPropertyChanged(nameof(Stage4Color));
                OnPropertyChanged(nameof(Stage5Color));
            }
        }
        
        private BookingDto? _bookingDetails;
        public BookingDto? BookingDetails 
        { 
            get => _bookingDetails;
            private set => SetProperty(ref _bookingDetails, value);
        }
        
        private string _userName = string.Empty;
        public string UserName 
        { 
            get => _userName;
            private set => SetProperty(ref _userName, value);
        }
        
        private string _serviceName = string.Empty;
        public string ServiceName 
        { 
            get => _serviceName;
            private set => SetProperty(ref _serviceName, value);
        }

        // Tracking properties
        public double Progress => CalculateProgress();
        public string Stage2Color => GetStageColor(2);
        public string Stage3Color => GetStageColor(3);
        public string Stage4Color => GetStageColor(4);
        public string Stage5Color => GetStageColor(5);

        private readonly List<string> StageSequence = new()
        {
            "Pending",
            "Started",
            "Inspection",
            "Completed",
            "Paid"
        };

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
            GoBackCommand = new Command(async () => 
            {
                try
                {
                    // Try to pop the current page
                    if (Shell.Current.Navigation.NavigationStack.Count > 1)
                    {
                        await Shell.Current.Navigation.PopAsync();
                    }
                    else
                    {
                        // If we can't pop, go to the booking list page
                        await Shell.Current.GoToAsync(nameof(StaffBookingListPage));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[GoBackCommand] Error: {ex.Message}");
                    // Fallback: navigate to booking list
                    await Shell.Current.GoToAsync(nameof(StaffBookingListPage));
                }
            });
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
                else
                {
                    // No stages exist yet, initialize with Pending
                    CurrentBooking = new BookingStageUpdateDto
                    {
                        BookingId = bookingId,
                        CurrentStage = "Pending",
                        AvailableStages = new List<string> { "Started", "Inspection", "Completed", "Paid" },
                        SelectedStage = string.Empty,
                        UpdatedAt = DateTime.UtcNow
                    };
                }
                
                // Make sure AvailableStages is populated (fallback)
                if (CurrentBooking?.AvailableStages == null || CurrentBooking.AvailableStages.Count == 0)
                {
                    CurrentBooking!.AvailableStages = new List<string> { "Started", "Inspection", "Completed", "Paid" };
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task UpdateStageAsync()
        {
            if (CurrentBooking is null)
            {
                ErrorMessage = "No booking selected.";
                return;
            }
            
            if (string.IsNullOrEmpty(CurrentBooking.SelectedStage))
            {
                ErrorMessage = "Please select a stage from the dropdown.";
                return;
            }

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
                ErrorMessage = "Failed to update status. Please try again.";
                return;
            }
            
            // Clear error on success
            ErrorMessage = string.Empty;
            StatusMessage = "Status updated successfully!";

            // Handle Completed stage - send notification to user
            if (CurrentBooking.SelectedStage == "Completed")
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

            // Handle Paid stage - award points to user
            if (CurrentBooking.SelectedStage == "Paid")
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

            // Update CurrentBooking with the new stage
            if (CurrentBooking != null)
            {
                CurrentBooking.CurrentStage = CurrentBooking.SelectedStage;
                CurrentBooking.UpdatedAt = DateTime.UtcNow;
                
                // Update UI for progress tracking
                OnPropertyChanged(nameof(CurrentBooking));
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(Stage2Color));
                OnPropertyChanged(nameof(Stage3Color));
                OnPropertyChanged(nameof(Stage4Color));
                OnPropertyChanged(nameof(Stage5Color));
            }
            
            // Navigate back to the booking list after a short delay to show success
            await Task.Delay(1000); // Brief pause to show success message
            
            try
            {
                // Try to pop the current page
                if (Shell.Current.Navigation.NavigationStack.Count > 1)
                {
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    // If we can't pop, go to the booking list page
                    await Shell.Current.GoToAsync(nameof(StaffBookingListPage));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UpdateStage] Navigation error: {ex.Message}");
                // Fallback: navigate to booking list
                await Shell.Current.GoToAsync(nameof(StaffBookingListPage));
            }
        }

        private double CalculateProgress()
        {
            if (CurrentBooking == null) return 0;
            var currentStage = CurrentBooking.CurrentStage ?? "Pending";
            var index = StageSequence.IndexOf(currentStage);
            if (index < 0) return 0;
            return (index + 1) / (double)StageSequence.Count;
        }

        private string GetStageColor(int stageIndex)
        {
            if (CurrentBooking == null) return "#3E3E3E";
            var currentStage = CurrentBooking.CurrentStage ?? "Pending";
            var currentIndex = StageSequence.IndexOf(currentStage);
            // Convert UI stage number (2,3,4,5) to array index (1,2,3,4)
            // because Stage 1 (Pending) is index 0 and always blue
            var arrayIndex = stageIndex - 1;
            if (arrayIndex <= currentIndex)
                return "#007AFF"; // Blue for completed stages
            else
                return "#3E3E3E"; // Gray for pending stages
        }
    }
}

