using TRAFFIK_APP.ViewModels;
namespace TRAFFIK_APP.Views
{
    public partial class SignupPage : ContentPage
    {
        public SignupPage(SignupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; 
        }

        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            var vm = BindingContext as SignupViewModel;
            vm.IsPasswordHidden = !vm.IsPasswordHidden;
        }

        private void ToggleConfirmPasswordVisibility(object sender, EventArgs e)
        {
            var vm = BindingContext as SignupViewModel;
            vm.IsConfirmPasswordHidden = !vm.IsConfirmPasswordHidden;
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}