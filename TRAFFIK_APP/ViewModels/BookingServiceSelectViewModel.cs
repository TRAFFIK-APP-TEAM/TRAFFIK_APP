using System.Collections.ObjectModel;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.ServiceCatalog;
using TRAFFIK_APP.Services.ApiClients;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingServiceSelectViewModel : BaseViewModel
    {
        private readonly ServiceCatalogClient _serviceCatalogClient;

        public ObservableCollection<ServiceCatalogDto> Services { get; } = new();

        private ServiceCatalogDto _selectedService;
        public ServiceCatalogDto SelectedService
        {
            get => _selectedService;
            set
            {
                if (_selectedService != value)
                {
                    _selectedService = value;
                    OnPropertyChanged(nameof(SelectedService));
                }
            }
        }

        public ICommand SelectServiceCommand { get; }
        public ICommand LoadServicesCommand { get; }

        public BookingServiceSelectViewModel(ServiceCatalogClient serviceCatalogClient)
        {
            _serviceCatalogClient = serviceCatalogClient;
            
            SelectServiceCommand = new Command<ServiceCatalogDto>(OnSelectService);
            LoadServicesCommand = new Command(() => ExecuteSafeAsync(LoadServicesAsync, "Loading services..."));

            // Load services on initialization
            _ = LoadServicesAsync();
        }

        private async Task LoadServicesAsync()
        {
            try
            {
                var serviceCatalogs = await _serviceCatalogClient.GetAllAsync();
                Services.Clear();
                
                if (serviceCatalogs != null)
                {
                    foreach (var service in serviceCatalogs)
                    {
                        Services.Add(service);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading services: {ex.Message}");
                ErrorMessage = "Failed to load services. Please try again.";
            }
        }

        private async void OnSelectService(ServiceCatalogDto? service)
        {
            if (service == null && SelectedService == null)
            {
                await Shell.Current.DisplayAlert("Select a Service", "Please select a service to continue.", "OK");
                return;
            }

            // Navigate to Vehicle selection page
            await Shell.Current.GoToAsync(nameof(TRAFFIK_APP.Views.BookingVehicleSelectPage));
        }
    }
}
