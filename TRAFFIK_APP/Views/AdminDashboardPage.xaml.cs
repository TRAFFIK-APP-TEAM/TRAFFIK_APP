using Microsoft.Maui.Controls;

namespace TRAFFIK_APP.Views
{
    public partial class AdminDashboardPage : ContentPage
    {
        public AdminDashboardPage()
        {
            InitializeComponent();
        }

        private async void OnViewAnalytics(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AdminAnalyticsPage));
        }

        private async void OnManageBookings(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AdminManageBookingsPage));
        }

        private async void OnManageUsers(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AdminManageUsersPage));
        }

        private async void OnAddAdmin(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddAdminPage));
        }

        private async void OnViewRedeemedRewards(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AdminManageRewardsPage));
        }
    }
}
