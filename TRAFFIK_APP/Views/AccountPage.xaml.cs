using TRAFFIK_APP.ViewModels;

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
}