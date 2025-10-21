using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;  // adjust if your models live elsewhere
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingConfirmationViewModel : BaseViewModel
    {
        public BookingServiceDto SelectedService { get; set; }
        public BookingVehicleDto SelectedVehicle { get; set; }
        public DateTime SelectedDate { get; set; }
        public TimeSpan SelectedTime { get; set; }

        public ICommand ConfirmBookingCommand { get; }
        public ICommand GoBackCommand { get; }

        public BookingConfirmationViewModel()
        {
            ConfirmBookingCommand = new Command(OnConfirmBooking);
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async void OnConfirmBooking()
        {
            // TODO: Send booking info to API or local service
            await Shell.Current.DisplayAlert("Success", "Your booking has been confirmed!", "OK");
            await Shell.Current.GoToAsync("//DashboardPage");
        }
    }
}
