using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class BookingDetailPage : ContentPage
    {
        public static TRAFFIK_APP.Models.Dtos.Booking.BookingDto? SelectedBooking { get; set; }
        private BookingDetailViewModel? _viewModel;

        public BookingDetailPage()
        {
            InitializeComponent();
            
            if (SelectedBooking != null)
            {
                _viewModel = new BookingDetailViewModel(SelectedBooking);
                BindingContext = _viewModel;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Reload stages when the page appears to show updated progress
            if (_viewModel != null)
            {
                await _viewModel.LoadStagesAsync();
            }
        }
    }
}
