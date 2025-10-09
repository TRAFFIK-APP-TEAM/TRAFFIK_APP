using TRAFFIK_APP.ViewModels;

namespace TRAFFIK_APP.Views
{
    public partial class DashboardPage : ContentPage
    {
        private DashboardViewModel ViewModel => BindingContext as DashboardViewModel;

        public DashboardPage(DashboardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, EventArgs e)
        {
            if (ViewModel?.LoadDashboardCommand.CanExecute(null) == true)
                ViewModel.LoadDashboardCommand.Execute(null);
        }

        private async void OnAddVehicleTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(AddVehiclePage));
        }
    }
}