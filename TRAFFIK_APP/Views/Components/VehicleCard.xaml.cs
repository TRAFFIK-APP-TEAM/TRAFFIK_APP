using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class VehicleCard : ContentView
{
    public VehicleCard()
    {
        InitializeComponent();
    }

    private async void OnClicked(EventArgs e)
    {
        await Navigation.PushAsync(new VehiclePage());
    }
}