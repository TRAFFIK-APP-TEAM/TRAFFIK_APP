using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Models.Entities.Vehicle;

namespace TRAFFIK_APP.Views;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	private async void OnNotificationClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(BookingTrackerPage));
	}

	private async void OnAddVehicleTapped(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(AddVehiclePage));
	}

	private async void OnVehicleTapped(object sender, EventArgs e)
	{
		if (sender is VisualElement element && element.BindingContext is Vehicle vehicle)
		{
			// Set the selected vehicle and navigate
			VehiclePage.SelectedVehicle = vehicle;
			await Shell.Current.GoToAsync(nameof(VehiclePage));
		}
	}
}