using TRAFFIK_APP.ViewModels;
namespace TRAFFIK_APP.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            var vm = BindingContext as LoginViewModel;
            vm.IsPasswordHidden = !vm.IsPasswordHidden;
        }

        private async void OnSignupTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SignupPage));
        }
    }
}