namespace TRAFFIK_APP.Views;

public partial class AdminAnalyticsPage : ContentPage
{
    public AdminAnalyticsPage()
    {
        InitializeComponent();
    }

    private async void OnAddAdminClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddAdminPage));
    }
}
