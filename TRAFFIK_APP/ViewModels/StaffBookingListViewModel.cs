using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Services.ApiClients;


namespace TRAFFIK_APP.ViewModels
{
    public class StaffBookingListViewModel : BaseViewModel
    {
        private readonly BookingStagesClient _bookingStagesClient;

        public ObservableCollection<BookingStageUpdateDto> AllBookings { get; } = new();

        public StaffBookingListViewModel(BookingStagesClient bookingStagesClient)
        {
            _bookingStagesClient = bookingStagesClient;
            _ = LoadAllBookingsAsync();
        }

        private async Task LoadAllBookingsAsync()
        {
            var items = await _bookingStagesClient.GetAllAsync();
            if (items is null) return;

            AllBookings.Clear();
            foreach (var item in items)
            {
                AllBookings.Add(item);
            }
        }
    }
}




