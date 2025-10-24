using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class AddVehiclePage : ContentPage
{
    public AddVehiclePage(AddVehicleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}
