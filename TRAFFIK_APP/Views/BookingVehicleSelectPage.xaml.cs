using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class BookingVehicleSelectPage : ContentPage
{
    public BookingVehicleSelectPage(BookingVehicleSelectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
