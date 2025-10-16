using System.Collections.ObjectModel;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingServiceSelectViewModel : BaseViewModel
    {
        public ObservableCollection<BookingServiceDto> Services { get; } = new();

        private BookingServiceDto _selectedService;
        public BookingServiceDto SelectedService
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

        public ICommand ContinueCommand { get; }

        public BookingServiceSelectViewModel()
        {
            // Mock data
            Services.Add(new BookingServiceDto { ServiceCatalogId = 1, ServiceName = "Car Wash", Description = "Full wash inside and out" });
            Services.Add(new BookingServiceDto { ServiceCatalogId = 2, ServiceName = "Oil Change", Description = "Engine oil & filter replacement" });
            Services.Add(new BookingServiceDto { ServiceCatalogId = 3, ServiceName = "Tyre Rotation", Description = "Rotate tyres for even wear" });

            ContinueCommand = new Command(OnContinue);
        }

        private async void OnContinue()
        {
            if (SelectedService == null)
            {
                await Shell.Current.DisplayAlert("Select a Service", "Please select a service to continue.", "OK");
                return;
            }

            // Navigate to Vehicle selection page
            await Shell.Current.GoToAsync(nameof(TRAFFIK_APP.Views.BookingVehicleSelectPage));
        }
    }
}
