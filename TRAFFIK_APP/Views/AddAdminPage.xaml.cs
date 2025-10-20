namespace TRAFFIK_APP.Views;

public partial class AddAdminPage : ContentPage
{
    public AddAdminPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AdminAnalyticsPage");
    }
}
