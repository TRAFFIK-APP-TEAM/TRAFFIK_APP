using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Services;
using TRAFFIK_APP.Helpers;

namespace TRAFFIK_APP.Views;

public partial class StaffProfilePage : ContentPage
{
	public StaffProfilePage(StaffProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		// Extra runtime guard in case of deep links
		var session = ServiceHelper.GetService<SessionService>();
		if (session is not null && !session.IsStaff)
		{
			_ = Shell.Current.DisplayAlert("Access Denied", "Staff only area.", "OK");
			_ = Shell.Current.GoToAsync("//DashboardPage");
		}
	}

	private async void OnBackClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}

