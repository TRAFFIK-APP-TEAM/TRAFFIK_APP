using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Models.Entities.Vehicle;

namespace TRAFFIK_APP.Views;

public partial class EditVehiclePage : ContentPage
{
    public static Vehicle? VehicleToEdit { get; set; }
    
    public EditVehiclePage(EditVehicleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (VehicleToEdit != null && BindingContext is EditVehicleViewModel viewModel)
        {
            viewModel.SetVehicle(VehicleToEdit);
        }
    }
}
