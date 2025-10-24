using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Specialized;
using TRAFFIK_APP.Models.Dtos.ServiceCatalog;
using TRAFFIK_APP.Models.Dtos.Vehicle;
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

        // Static property to hold selected vehicle between pages
        public static VehicleDto SelectedVehicle { get; set; }

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

        public string SelectedVehicleDisplayName => SelectedVehicle?.DisplayName ?? "No Vehicle Selected";


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
            GoBackCommand = new Command(async () => 
            {
                try
                {
                    await Shell.Current.GoToAsync("..");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in GoBackCommand: {ex.Message}");
                    await Shell.Current.GoToAsync(nameof(TRAFFIK_APP.Views.BookingVehicleSelectPage));
                }
            });
            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));

            // Load services on initialization - but only if we have a vehicle selected
            if (SelectedVehicle != null)
            {
                _ = LoadServicesAsync();
            }
        }

        // Method to reload services when vehicle changes
        public void ReloadServicesForVehicle()
        {
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
                        // Filter services based on vehicle type compatibility
                        if (IsServiceCompatibleWithVehicle(serviceDto, SelectedVehicle))
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
                            System.Diagnostics.Debug.WriteLine($"Added compatible service: {serviceItem.Name} - {serviceItem.Price}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Skipped incompatible service: {serviceDto.Name} for vehicle type: {SelectedVehicle?.VehicleType}");
                        }
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"Total compatible services in collection: {Services.Count}");
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

        private bool IsServiceCompatibleWithVehicle(ServiceCatalogDto service, VehicleDto? vehicle)
        {
            if (vehicle == null) 
            {
                System.Diagnostics.Debug.WriteLine("IsServiceCompatibleWithVehicle: No vehicle provided");
                return false;
            }
            
            var vehicleType = vehicle.VehicleType?.ToLowerInvariant() ?? "";
            var serviceName = service.Name?.ToLowerInvariant() ?? "";
            var serviceDescription = service.Description?.ToLowerInvariant() ?? "";
            
            System.Diagnostics.Debug.WriteLine($"Checking compatibility: Vehicle={vehicleType}, Service={serviceName}");
            
            // Define compatibility rules based on vehicle type and service
            switch (vehicleType)
            {
                case "sedan":
                case "hatchback":
                case "coupe":
                case "car":
                    // Most services are compatible with smaller vehicles
                    System.Diagnostics.Debug.WriteLine($"Sedan/Hatchback/Coupe: Allowing service {serviceName}");
                    return true;
                    
                case "suv":
                case "truck":
                case "pickup":
                case "van":
                    // Larger vehicles - most services are fine
                    System.Diagnostics.Debug.WriteLine($"SUV/Truck/Pickup: Allowing service {serviceName}");
                    return true;
                    
                case "luxury":
                case "sports car":
                case "supercar":
                case "premium":
                    // High-end vehicles - prefer premium services
                    System.Diagnostics.Debug.WriteLine($"Luxury/Sports: Allowing service {serviceName}");
                    return true;
                    
                case "motorcycle":
                case "bike":
                case "motorbike":
                    // Motorcycles - only basic services
                    if (serviceName.Contains("express") || serviceName.Contains("basic") || 
                        serviceName.Contains("wash") || serviceName.Contains("quick"))
                    {
                        System.Diagnostics.Debug.WriteLine($"Motorcycle: Allowing basic service {serviceName}");
                        return true;
                    }
                    System.Diagnostics.Debug.WriteLine($"Motorcycle: Excluding complex service {serviceName}");
                    return false;
                    
                default:
                    // Unknown vehicle type - allow all services for now
                    System.Diagnostics.Debug.WriteLine($"Unknown vehicle type '{vehicleType}': Allowing service {serviceName}");
                    return true;
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
            try
            {
                if (service == null)
                {
                    await Shell.Current.DisplayAlert("Select a Service", "Please select a service to continue.", "OK");
                    return;
                }

                if (SelectedVehicle == null)
                {
                    await Shell.Current.DisplayAlert("No Vehicle Selected", "Please go back and select a vehicle first.", "OK");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Selected service: {service.Name}");
                System.Diagnostics.Debug.WriteLine($"Selected vehicle: {SelectedVehicle.DisplayName}");

                SelectedService = service;
                
                // Store the selected service and vehicle in static properties for the next page
                BookingDateTimeSelectViewModel.SelectedService = service;
                BookingDateTimeSelectViewModel.SelectedVehicle = SelectedVehicle;
                
                System.Diagnostics.Debug.WriteLine("Navigating to DateTime selection page...");
                System.Diagnostics.Debug.WriteLine($"Passing vehicle: {SelectedVehicle?.DisplayName}");
                System.Diagnostics.Debug.WriteLine($"Passing service: {service.Name}");
                
                // Navigate to DateTime selection page
                await Shell.Current.GoToAsync(nameof(TRAFFIK_APP.Views.BookingDateTimeSelectPage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnSelectService: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                await Shell.Current.DisplayAlert("Error", "Failed to select service. Please try again.", "OK");
            }
        }
    }
}
