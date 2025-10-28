using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class StaffBookingListPage : ContentPage
    {
        public StaffBookingListPage(StaffBookingListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}

