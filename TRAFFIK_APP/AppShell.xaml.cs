using TRAFFIK_APP.Helpers;
using TRAFFIK_APP.Services;
using TRAFFIK_APP.Views;

namespace TRAFFIK_APP
{
	public partial class AppShell : Shell
	{
		private readonly SessionService _sessionService;

		public AppShell()
		{
			InitializeComponent();
			_sessionService = ServiceHelper.GetService<SessionService>();

            Routing.RegisterRoute(nameof(AdminDashboardPage), typeof(AdminDashboardPage));
            Routing.RegisterRoute(nameof(AdminAnalyticsPage), typeof(AdminAnalyticsPage));
            Routing.RegisterRoute(nameof(AdminManageBookingsPage), typeof(AdminManageBookingsPage));
            Routing.RegisterRoute(nameof(AdminManageUsersPage), typeof(AdminManageUsersPage));
            Routing.RegisterRoute(nameof(AddAdminPage), typeof(AddAdminPage));
            Routing.RegisterRoute(nameof(AdminManageRewardsPage), typeof(AdminManageRewardsPage));
            Routing.RegisterRoute("VehiclePage", typeof(VehiclePage));
            Routing.RegisterRoute(nameof(BookingTrackerPage), typeof(BookingTrackerPage));
            
            // Booking flow pages
            Routing.RegisterRoute(nameof(BookingVehicleSelectPage), typeof(BookingVehicleSelectPage));
            Routing.RegisterRoute(nameof(BookingServiceSelectPage), typeof(BookingServiceSelectPage));
            Routing.RegisterRoute(nameof(BookingDateTimeSelectPage), typeof(BookingDateTimeSelectPage));
            Routing.RegisterRoute(nameof(BookingConfirmationPage), typeof(BookingConfirmationPage));
            
            // Staff booking pages
            Routing.RegisterRoute(nameof(StaffBookingListPage), typeof(StaffBookingListPage));
            Routing.RegisterRoute(nameof(StaffBookingDetailPage), typeof(StaffBookingDetailPage));

























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
				// Redirect based on user role
				if (_sessionService.IsAdmin)
					await Shell.Current.GoToAsync("//AdminDashboardPage");
				else if (_sessionService.IsStaff)
					await Shell.Current.GoToAsync("//StaffDashboardPage");
				else
					await Shell.Current.GoToAsync("//DashboardPage");
				return;
			}
			
			// Guard StaffProfilePage
			if (target.Contains("StaffProfilePage", StringComparison.OrdinalIgnoreCase) && !_sessionService.IsStaff)
			{
				args.Cancel();
				await Shell.Current.DisplayAlert("Access Denied", "Staff only area.", "OK");
				// Redirect based on user role
				if (_sessionService.IsAdmin)
					await Shell.Current.GoToAsync("//AdminDashboardPage");
				else if (_sessionService.IsStaff)
					await Shell.Current.GoToAsync("//StaffDashboardPage");
				else
					await Shell.Current.GoToAsync("//DashboardPage");
				return;
			}

			// Guard all Admin pages
			if ((target.Contains("AdminDashboardPage", StringComparison.OrdinalIgnoreCase)
				|| target.Contains("AdminAnalyticsPage", StringComparison.OrdinalIgnoreCase)
				|| target.Contains("AddAdminPage", StringComparison.OrdinalIgnoreCase))
				&& !_sessionService.IsAdmin)
			{
				args.Cancel();
				await Shell.Current.DisplayAlert("Access Denied", "Admin only area.", "OK");
				// Redirect based on user role
				if (_sessionService.IsAdmin)
					await Shell.Current.GoToAsync("//AdminDashboardPage");
				else if (_sessionService.IsStaff)
					await Shell.Current.GoToAsync("//StaffDashboardPage");
				else
					await Shell.Current.GoToAsync("//DashboardPage");
				return;
			}
		}
	}
}
