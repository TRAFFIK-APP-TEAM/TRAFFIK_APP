using TRAFFIK_APP.ViewModels;
using TRAFFIK_APP.Models;
using System.Collections.ObjectModel;

namespace TRAFFIK_APP.Views
{
    public partial class DashboardPage : ContentPage
    {
        private DashboardViewModel ViewModel => BindingContext as DashboardViewModel;

        public ObservableCollection<object> VehicleCards { get; } = new();

        public DashboardPage(DashboardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            this.BindingContext = this;
            Loaded += OnPageLoaded;
        }

        private async void OnPageLoaded(object sender, EventArgs e)
        {
            if (ViewModel?.LoadDashboardCommand?.CanExecute(null) == true)
                ViewModel.LoadDashboardCommand.Execute(null);

            UpdateVehicleCards();
        }

        private void UpdateVehicleCards()
        {
            VehicleCards.Clear();
            VehicleCards.Add(null); // Add card placeholder!

            if (ViewModel?.Vehicles != null)
            {
                foreach (var vehicle in ViewModel.Vehicles)
                {
                    VehicleCards.Add(vehicle);
                }
            }
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
                var route = $"{nameof(VehiclePage)}?vehicleId={vehicle.Id}";
                await Shell.Current.GoToAsync(route);
            }
        }
    }
}