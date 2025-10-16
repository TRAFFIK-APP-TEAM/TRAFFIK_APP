using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class BookingTrackerPage : ContentPage
    {
        public BookingTrackerPage()
        {
            InitializeComponent();
            BindingContext = new BookingTrackerViewModel();
        }
    }
}