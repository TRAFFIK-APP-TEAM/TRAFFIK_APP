using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class StaffBookingDetailPage : ContentPage
    {
        private StaffBookingDetailViewModel? _viewModel;

        public static BookingStageUpdateDto? SelectedBooking { get; set; }

        public StaffBookingDetailPage(StaffBookingDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Load booking data from static property
            if (SelectedBooking != null && _viewModel != null)
            {
                await _viewModel.LoadBookingDetailsAsync(SelectedBooking.BookingId);
            }
        }
    }
}

