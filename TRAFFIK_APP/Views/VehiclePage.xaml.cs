using TRAFFIK_APP.Models.Entities.Vehicle;
using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class VehiclePage : ContentPage
{
    private Vehicle? _vehicle;
    private VehicleViewModel? _viewModel;
    
    // Static property to hold the selected vehicle
    public static Vehicle? SelectedVehicle { get; set; }

    public VehiclePage()
    {
        InitializeComponent();
        _viewModel = new VehicleViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        // Load vehicle data from static property
        if (SelectedVehicle != null)
        {
            _vehicle = SelectedVehicle;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (_vehicle != null)
        {
            LicensePlateLabel.Text = _vehicle.LicensePlate;
            MakeLabel.Text = _vehicle.Make;
            ModelLabel.Text = _vehicle.Model;
            TypeLabel.Text = _vehicle.VehicleType;
            ColorLabel.Text = _vehicle.Color;
            VehicleImage.Source = _vehicle.ImageUrl;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnBookServiceClicked(object sender, EventArgs e)
    {
        if (_vehicle != null)
        {
            // Navigate to booking page (you can implement this later)
            await Shell.Current.GoToAsync("//BookingPage");
        }
    }

    private async void OnViewHistoryClicked(object sender, EventArgs e)
    {
        // Navigate to service history page (you can implement this later)
        await DisplayAlert("Service History", "Service history feature coming soon!", "OK");
    }
}