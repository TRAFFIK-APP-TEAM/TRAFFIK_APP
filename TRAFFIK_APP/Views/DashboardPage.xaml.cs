using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Models;
using System.Collections.ObjectModel;
using TRAFFIK_APP.Models.Entities.Vehicle;

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
            if (ViewModel?.LoadDashboardCommand?.CanExecute(null) == true)
                ViewModel.LoadDashboardCommand.Execute(null);
        }

        private async void OnAddVehicleTapped(object sender, EventArgs e)
        {
            AddCardBorder.Stroke = Colors.Blue;
            await Shell.Current.GoToAsync(nameof(AddVehiclePage));
        }

        private async void OnVehicleTapped(object sender, EventArgs e)
        {
            if (sender is VisualElement element && element.BindingContext is Vehicle vehicle)
            {
                // Set the selected vehicle and navigate
                VehiclePage.SelectedVehicle = vehicle;
                await Shell.Current.GoToAsync(nameof(VehiclePage));
            }
        }

        private async void OnNotificationClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(BookingTrackerPage));
        }
    }
}