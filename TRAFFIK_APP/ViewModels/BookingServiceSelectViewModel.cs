using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Specialized;
using TRAFFIK_APP.Models.Dtos.ServiceCatalog;
using TRAFFIK_APP.Services.ApiClients;
using System.Text.Json;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingServiceSelectViewModel : BaseViewModel
    {
        private readonly ServiceCatalogClient _serviceCatalogClient;
        private string _searchText = string.Empty;
        private string _selectedCategory = "All";
        private bool _isRefreshing;

        public ObservableCollection<ServiceCatalogItem> Services { get; } = new();
        public ObservableCollection<ServiceCatalogItem> FilteredServices { get; } = new();

        private ServiceCatalogItem _selectedService;
        public ServiceCatalogItem SelectedService
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


        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    OnPropertyChanged(nameof(HasSearchText));
                    FilterServices();
                }
            }
        }

        public bool HasSearchText => !string.IsNullOrWhiteSpace(SearchText);

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    UpdateCategorySelection();
                    FilterServices();
                }
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        // Category selection properties
        public bool AllSelected => SelectedCategory == "All";
        public bool BasicSelected => SelectedCategory == "Basic";
        public bool ValetSelected => SelectedCategory == "Valet";
        public bool PolishSelected => SelectedCategory == "Polish";

        public int ServiceCount => FilteredServices.Count;

        public ICommand SelectServiceCommand { get; }
        public ICommand LoadServicesCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand FilterByCategoryCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand GoHomeCommand { get; }
        public ICommand GoAppointmentsCommand { get; }
        public ICommand GoRewardsCommand { get; }
        public ICommand GoAccountCommand { get; }

        public BookingServiceSelectViewModel(ServiceCatalogClient serviceCatalogClient)
        {
            _serviceCatalogClient = serviceCatalogClient;
            
            SelectServiceCommand = new Command<ServiceCatalogItem>(OnSelectService);
            LoadServicesCommand = new Command(() => ExecuteSafeAsync(LoadServicesAsync, "Loading services..."));
            RefreshCommand = new Command(() => ExecuteSafeAsync(RefreshServicesAsync, "Refreshing services..."));
            ClearSearchCommand = new Command(() => SearchText = string.Empty);
            FilterByCategoryCommand = new Command<string>(OnFilterByCategory);
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));

            // Load services on initialization
            _ = LoadServicesAsync();
        }

        private async Task LoadServicesAsync()
        {
            try
            {
                // Use the ServiceCatalogClient to get services
                var serviceDtos = await _serviceCatalogClient.GetAllAsync();
                System.Diagnostics.Debug.WriteLine($"ServiceCatalogClient returned {serviceDtos?.Count ?? 0} services");
                
                Services.Clear();
                
                if (serviceDtos != null)
                {
                    foreach (var serviceDto in serviceDtos)
                    {
                        var serviceItem = new ServiceCatalogItem
                        {
                            Id = serviceDto.Id,
                            Name = serviceDto.Name,
                            Description = serviceDto.Description,
                            Price = serviceDto.Price,
                            CarTypeId = null,
                            Category = GetServiceCategory(serviceDto.Name),
                            EstimatedDurationMinutes = GetEstimatedDuration(serviceDto.Name)
                        };
                        Services.Add(serviceItem);
                        System.Diagnostics.Debug.WriteLine($"Added service: {serviceItem.Name} - {serviceItem.Price}");
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"Total services in collection: {Services.Count}");
                FilterServices();
                System.Diagnostics.Debug.WriteLine($"Filtered services: {FilteredServices.Count}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading services: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ErrorMessage = "Failed to load services. Please try again.";
            }
        }

        private async Task RefreshServicesAsync()
        {
            IsRefreshing = true;
            await LoadServicesAsync();
            IsRefreshing = false;
        }

        private void OnFilterByCategory(string category)
        {
            SelectedCategory = category;
        }

        private void UpdateCategorySelection()
        {
            OnPropertyChanged(nameof(AllSelected));
            OnPropertyChanged(nameof(BasicSelected));
            OnPropertyChanged(nameof(ValetSelected));
            OnPropertyChanged(nameof(PolishSelected));
        }

        private void FilterServices()
        {
            FilteredServices.Clear();

            var filtered = Services.Where(service =>
            {
                // Category filter
                bool categoryMatch = SelectedCategory == "All" || 
                    (SelectedCategory == "Basic" && service.Category.Contains("Basic")) ||
                    (SelectedCategory == "Valet" && service.Category.Contains("Valet")) ||
                    (SelectedCategory == "Polish" && service.Category.Contains("Polish"));

                // Search filter
                bool searchMatch = string.IsNullOrWhiteSpace(SearchText) ||
                    service.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    service.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    service.Category.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

                return categoryMatch && searchMatch;
            }).OrderBy(s => s.Name);

            foreach (var service in filtered)
            {
                FilteredServices.Add(service);
            }

            OnPropertyChanged(nameof(ServiceCount));
        }

        private string GetServiceCategory(string serviceName)
        {
            if (serviceName.Contains("Traffik Wash") || serviceName.Contains("Express Wash") || serviceName.Contains("Wash & Go"))
                return "Basic Wash";
            if (serviceName.Contains("Valet") || serviceName.Contains("Mini Valet") || serviceName.Contains("Deluxe Valet"))
                return "Valet";
            if (serviceName.Contains("Polish") || serviceName.Contains("Claybar") || serviceName.Contains("Machine"))
                return "Polish";
            if (serviceName.Contains("Decontamination") || serviceName.Contains("Chassis"))
                return "Deep Clean";
            return "Other";
        }

        private int GetEstimatedDuration(string serviceName)
        {
            if (serviceName.Contains("Express") || serviceName.Contains("Wash & Go"))
                return 30;
            if (serviceName.Contains("Traffik Wash"))
                return 90;
            if (serviceName.Contains("Mini Valet"))
                return 120;
            if (serviceName.Contains("Valet"))
                return 180;
            if (serviceName.Contains("Polish"))
                return 240;
            return 60; // Default
        }

        private async void OnSelectService(ServiceCatalogItem? service)
        {
            if (service == null)
            {
                await Shell.Current.DisplayAlert("Select a Service", "Please select a service to continue.", "OK");
                return;
            }

            SelectedService = service;
            
            // Navigate to Vehicle selection page
            await Shell.Current.GoToAsync(nameof(TRAFFIK_APP.Views.BookingVehicleSelectPage));
        }
    }
}
