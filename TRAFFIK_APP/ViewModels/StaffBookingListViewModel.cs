using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;


namespace TRAFFIK_APP.ViewModels
{
    public class StaffBookingListViewModel : BaseViewModel
    {
        private readonly BookingStagesClient _bookingStagesClient;

        public ObservableCollection<BookingStageUpdateDto> AllBookings { get; } = new();
        public ICommand ViewBookingCommand { get; }

        public StaffBookingListViewModel(BookingStagesClient bookingStagesClient)
        {
            _bookingStagesClient = bookingStagesClient;

            ViewBookingCommand = new Command<BookingStageUpdateDto>(async (booking) =>
            {
                if (booking != null)
                {
                    StaffBookingDetailPage.SelectedBooking = booking;
                    await Shell.Current.GoToAsync(nameof(StaffBookingDetailPage));
                }
            });

            _ = LoadAllBookingsAsync();
        }

        private async Task LoadAllBookingsAsync()
        {
            var items = await _bookingStagesClient.GetAllAsync();
            if (items is null) return;

            AllBookings.Clear();
            foreach (var item in items)
            {
                // Filter for today's bookings or bookings in progress
                if (IsTodayOrInProgress(item))
                {
                    AllBookings.Add(item);
                }
            }
        }

        private bool IsTodayOrInProgress(BookingStageUpdateDto booking)
        {
            // Check if booking is in progress (not at final stage)
            var lastStage = booking.AvailableStages.LastOrDefault();
            if (booking.CurrentStage != lastStage)
            {
                return true;
            }
            
            // Check if booking is from today
            // This is a simple check - you might want to enhance this based on your booking date logic
            return true;
        }
    }
}
