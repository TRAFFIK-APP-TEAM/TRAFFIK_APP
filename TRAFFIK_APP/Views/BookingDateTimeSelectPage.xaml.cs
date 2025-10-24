
using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class BookingDateTimeSelectPage : ContentPage
{
    public BookingDateTimeSelectPage(BookingDateTimeSelectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
