namespace TRAFFIK_APP.Views;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage()
    {
        InitializeComponent();
    }

    private async void OnViewAnalytics(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AdminAnalyticsPage");
    }

    private async void OnManageBookings(object sender, EventArgs e)
    {
        // Navigate to a bookings management page
        // For now, show a simple alert since we don't have a dedicated bookings management page
        await DisplayAlert("Manage Bookings", "Bookings management feature coming soon!", "OK");
    }

    private async void OnManageUsers(object sender, EventArgs e)
    {
        // Navigate to a users management page
        // For now, show a simple alert since we don't have a dedicated users management page
        await DisplayAlert("Manage Users", "Users management feature coming soon!", "OK");
    }
}
