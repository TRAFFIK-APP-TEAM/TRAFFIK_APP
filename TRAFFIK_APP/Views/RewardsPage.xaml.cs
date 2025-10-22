using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class RewardsPage : ContentPage
{
    private RewardsViewModel ViewModel => BindingContext as RewardsViewModel;

    public RewardsPage(RewardsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object sender, EventArgs e)
    {
        if (ViewModel?.RefreshCommand?.CanExecute(null) == true)
            ViewModel.RefreshCommand.Execute(null);
    }
}