using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class BookingPage : ContentPage
{
    public BookingPage(BookingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
