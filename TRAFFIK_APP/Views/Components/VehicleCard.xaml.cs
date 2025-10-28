using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Models.Entities.Vehicle;

namespace TRAFFIK_APP.Views.Components;

public partial class VehicleCard : ContentView
{
    public VehicleCard()
    {
        InitializeComponent();
        System.Diagnostics.Debug.WriteLine("[VehicleCard] Constructor called");
        
        // Force a layout update to make sure the card is visible
        this.Loaded += (s, e) => {
            System.Diagnostics.Debug.WriteLine("[VehicleCard] Loaded event fired");
            this.InvalidateMeasure();
            this.InvalidateLayout();
        };
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (BindingContext is Vehicle vehicle)
        {
            System.Diagnostics.Debug.WriteLine($"[VehicleCard] BindingContext changed to vehicle: {vehicle.Make} {vehicle.Model} ({vehicle.LicensePlate})");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"[VehicleCard] BindingContext changed to: {BindingContext?.GetType().Name ?? "null"}");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        System.Diagnostics.Debug.WriteLine($"[VehicleCard] OnAppearing called with BindingContext: {BindingContext?.GetType().Name ?? "null"}");
    }
}