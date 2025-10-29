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
            // AdminAnalyticsPage is a ShellContent, use absolute navigation
            await Shell.Current.GoToAsync("//AdminAnalyticsPage");
        }

        private async void OnManageBookings(object sender, EventArgs e)
        {
            // Navigate using unambiguous route name
            await Shell.Current.GoToAsync("admin_manage_bookings");
        }

        private async void OnManageUsers(object sender, EventArgs e)
        {
            // Navigate using unambiguous route name
            await Shell.Current.GoToAsync("admin_manage_users");
        }

        private async void OnAddAdmin(object sender, EventArgs e)
        {
            // AddAdminPage is a ShellContent, use absolute navigation
            await Shell.Current.GoToAsync("//AddAdminPage");
        }

        private async void OnViewRedeemedRewards(object sender, EventArgs e)
        {
            // Navigate using unambiguous route name
            await Shell.Current.GoToAsync("admin_manage_rewards");
        }
    }
}
