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
            var viewModel = new AdminManageUsersViewModel(userClient, sessionService);
            BindingContext = viewModel;
            
            // Wire up the Shell navigation bar back button
            Shell.SetBackButtonBehavior(this, new Microsoft.Maui.Controls.BackButtonBehavior
            {
                Command = viewModel.GoBackCommand
            });
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

        protected override bool OnBackButtonPressed()
        {
            // Navigate back when hardware/gesture back button is pressed
            Shell.Current.GoToAsync("..").Wait();
            return true; // Prevent default back button behavior
        }
    }
}
