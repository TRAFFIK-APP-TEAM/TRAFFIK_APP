using TRAFFIK_APP.Services;
using TRAFFIK_APP.Helpers;

namespace TRAFFIK_APP
{
	public partial class AppShell : Shell
	{
		private readonly SessionService _sessionService;

		public AppShell()
		{
			InitializeComponent();
			_sessionService = ServiceHelper.GetService<SessionService>();
		}

		protected override async void OnNavigating(ShellNavigatingEventArgs args)
		{
			base.OnNavigating(args);
			if (_sessionService is null) return;

			var target = args.Target?.Location?.OriginalString ?? string.Empty;
			if (string.IsNullOrWhiteSpace(target)) return;

			// Enforce strict role-based navigation
			if (target.Contains("StaffDashboardPage", StringComparison.OrdinalIgnoreCase) && !_sessionService.IsStaff)
			{
				args.Cancel();
				await Shell.Current.DisplayAlert("Access Denied", "Staff only area.", "OK");
				await Shell.Current.GoToAsync("//UserDashboardPage");
				return;
			}

			if (target.Contains("AdminDashboardPage", StringComparison.OrdinalIgnoreCase) && !_sessionService.IsAdmin)
			{
				args.Cancel();
				await Shell.Current.DisplayAlert("Access Denied", "Admin only area.", "OK");
				await Shell.Current.GoToAsync("//UserDashboardPage");
				return;
			}
		}
	}
}
