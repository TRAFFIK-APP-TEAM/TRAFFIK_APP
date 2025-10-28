using TRAFFIK_APP.Models.Entities.Vehicle;
using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.Views;

public partial class VehiclePage : ContentPage
{
    private Vehicle? _vehicle;
    private VehicleViewModel? _viewModel;
    private readonly VehicleClient _vehicleClient;
    private readonly SessionService _session;
    
    // Static property to hold the selected vehicle
    public static Vehicle? SelectedVehicle { get; set; }

    public VehiclePage(VehicleClient vehicleClient, SessionService session)
    {
        InitializeComponent();
        _vehicleClient = vehicleClient;
        _session = session;
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

    private async void OnEditVehicleClicked(object sender, EventArgs e)
    {
        if (_vehicle != null)
        {
            // Set the vehicle to edit in the EditVehiclePage
            EditVehiclePage.VehicleToEdit = _vehicle;
            await Shell.Current.GoToAsync(nameof(EditVehiclePage));
        }
    }

    private async void OnDeleteVehicleClicked(object sender, EventArgs e)
    {
        if (_vehicle == null) return;

        var result = await DisplayAlert(
            "Delete Vehicle", 
            "Are you sure you want to delete this vehicle? This action cannot be undone.", 
            "Delete", 
            "Cancel");

        if (!result) return;

        try
        {
            var success = await _vehicleClient.DeleteAsync(_vehicle.LicensePlate);
            
            if (success)
            {
                await DisplayAlert("Success", "Vehicle deleted successfully!", "OK");
                await Shell.Current.GoToAsync("//DashboardPage");
            }
            else
            {
                await DisplayAlert("Error", "Failed to delete vehicle. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[VehiclePage] Error deleting vehicle: {ex.Message}");
            await DisplayAlert("Error", "An error occurred while deleting the vehicle.", "OK");
        }
    }
}