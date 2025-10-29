using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class BookingDetailPage : ContentPage
    {
        public static TRAFFIK_APP.Models.Dtos.Booking.BookingDto? SelectedBooking { get; set; }
        public static string? SourcePage { get; set; } // Track where we came from
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
            
            // Update back command based on source
            if (_viewModel != null && !string.IsNullOrEmpty(SourcePage))
            {
                if (SourcePage == "AdminManageBookingsPage")
                {
                    _viewModel.SetBackNavigation(async () => await Shell.Current.GoToAsync(".."));
                }
                // Else use default PopAsync behavior
            }
        }
    }
}
