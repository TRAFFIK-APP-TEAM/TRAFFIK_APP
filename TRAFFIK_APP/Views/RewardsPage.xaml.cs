using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views;

public partial class RewardsPage : ContentPage
{
    public RewardsPage(RewardsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
