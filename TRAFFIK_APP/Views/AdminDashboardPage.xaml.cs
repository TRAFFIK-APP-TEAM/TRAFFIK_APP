namespace Project_Jeremy.Presentation.Pages.Admin;

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
}
