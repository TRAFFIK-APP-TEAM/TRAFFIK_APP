using System;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Vehicle;
using TRAFFIK_APP.Models.Dtos.ServiceCatalog;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingDateTimeSelectViewModel : BaseViewModel
    {
        private DateTime _selectedDate = DateTime.Today;
        private TimeSpan _selectedTime = DateTime.Now.TimeOfDay;

        // Static properties to hold selected data between pages
        public static VehicleDto SelectedVehicle { get; set; }
        public static ServiceCatalogItem SelectedService { get; set; }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public TimeSpan SelectedTime
        {
            get => _selectedTime;
            set => SetProperty(ref _selectedTime, value);
        }

        public DateTime Today => DateTime.Today;

        public string SelectedVehicleDisplayName => SelectedVehicle?.DisplayName ?? "No Vehicle Selected";
        public string SelectedServiceDisplayName => SelectedService?.Name ?? "No Service Selected";

        public ICommand ContinueCommand { get; }

        public BookingDateTimeSelectViewModel()
        {
            ContinueCommand = new Command(async () => await OnContinue());
        }

        private async Task OnContinue()
        {
            if (SelectedVehicle == null || SelectedService == null)
            {
                await Shell.Current.DisplayAlert("Missing Information", "Please go back and select both a vehicle and service.", "OK");
                return;
            }

            // Combine date + time for confirmation
            var selectedDateTime = SelectedDate.Date + SelectedTime;
            
            // Store the selected data in static properties for the next page
            BookingConfirmationViewModel.SelectedVehicle = SelectedVehicle;
            BookingConfirmationViewModel.SelectedService = SelectedService;
            BookingConfirmationViewModel.SelectedDateTime = selectedDateTime;
            
            // Navigate to confirmation page
            await Shell.Current.GoToAsync($"/BookingConfirmationPage");
        }
    }
}
