using TRAFFIK_APP.Services;
using TRAFFIK_APP.Helpers;
using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
	public partial class StaffDashboardPage : ContentPage
	{
		public StaffDashboardPage(StaffDashboardViewModel viewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
			// Extra runtime guard in case of deep links
			var session = ServiceHelper.GetService<SessionService>();
			if (session is not null && !session.IsStaff)
			{
				_ = Shell.Current.DisplayAlert("Access Denied", "Staff only area.", "OK");
				// Redirect based on user role
				if (session.IsAdmin)
					_ = Shell.Current.GoToAsync("//AdminDashboardPage");
				else if (session.IsStaff)
					_ = Shell.Current.GoToAsync("//StaffDashboardPage");
				else
					_ = Shell.Current.GoToAsync("//DashboardPage");
			}
		}
	}
}