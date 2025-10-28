using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class BookingDetailPage : ContentPage
    {
        public static TRAFFIK_APP.Models.Dtos.Booking.BookingDto? SelectedBooking { get; set; }

        public BookingDetailPage()
        {
            InitializeComponent();
            
            if (SelectedBooking != null)
            {
                BindingContext = new BookingDetailViewModel(SelectedBooking);
            }
        }
    }
}
