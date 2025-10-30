using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class AddAdminPage : ContentPage
{
    public AddAdminPage(AddAdminViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AdminDashboardPage");
    }
}
