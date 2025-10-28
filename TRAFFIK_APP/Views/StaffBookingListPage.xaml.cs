using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class StaffBookingListPage : ContentPage
    {
        private StaffBookingListViewModel? _viewModel;

        public StaffBookingListPage(StaffBookingListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Reload bookings when the page appears to show updated statuses
            if (_viewModel != null)
            {
                await _viewModel.LoadAllBookingsAsync();
            }
        }
    }
}

