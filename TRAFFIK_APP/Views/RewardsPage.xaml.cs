using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class RewardsPage : ContentPage
{
    private RewardsViewModel? ViewModel => BindingContext as RewardsViewModel;

    public RewardsPage(RewardsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Reload rewards when page appears to ensure fresh data
        if (ViewModel != null && !ViewModel.IsBusy)
        {
            await ViewModel.LoadRewardsAsync();
        }
    }
}