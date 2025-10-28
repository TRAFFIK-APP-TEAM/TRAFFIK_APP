using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.Views
{
    public partial class AdminManageRewardsPage : ContentPage
    {
        public AdminManageRewardsPage()
        {
            InitializeComponent();
            
            // Get services from DI container
            var catalogClient = Application.Current.Handler.MauiContext.Services.GetService<RewardCatalogClient>();
            var sessionService = Application.Current.Handler.MauiContext.Services.GetService<SessionService>();
            
            // Set the ViewModel
            BindingContext = new AdminManageRewardsViewModel(catalogClient, sessionService);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Load redeemed rewards when page appears
            if (BindingContext is AdminManageRewardsViewModel viewModel)
            {
                await viewModel.LoadRedeemedRewardsAsync();
            }
        }
    }
}

