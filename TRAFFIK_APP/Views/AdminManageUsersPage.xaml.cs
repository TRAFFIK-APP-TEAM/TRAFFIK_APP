using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.Views
{
    public partial class AdminManageUsersPage : ContentPage
    {
        public AdminManageUsersPage()
        {
            InitializeComponent();
            
            // Get services from DI container
            var userClient = Application.Current.Handler.MauiContext.Services.GetService<UserClient>();
            var sessionService = Application.Current.Handler.MauiContext.Services.GetService<SessionService>();
            
            // Set the ViewModel
            BindingContext = new AdminManageUsersViewModel(userClient, sessionService);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            // Load users when page appears
            if (BindingContext is AdminManageUsersViewModel viewModel)
            {
                viewModel.LoadUsersCommand.Execute(null);
            }

        }
    }
}
