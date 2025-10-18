using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Views;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingVehicleSelectViewModel : BaseViewModel
    {
        // Vehicle list
        public ObservableCollection<BookingVehicleDto> Vehicles { get; } = new();

        // Commands
        public Command<BookingVehicleDto> SelectVehicleCommand { get; }
        public Command GoHomeCommand { get; }
        public Command GoAppointmentsCommand { get; }
        public Command GoRewardsCommand { get; }
        public Command GoAccountCommand { get; }

        public BookingVehicleSelectViewModel()
        {
            // Placeholder mock data
            Vehicles.Add(new BookingVehicleDto { UserId = 1, VehicleDisplayName = "Toyota Supra (Red)" });
            Vehicles.Add(new BookingVehicleDto { UserId = 2, VehicleDisplayName = "Nissan GT-R (Silver)" });
            Vehicles.Add(new BookingVehicleDto { UserId = 3, VehicleDisplayName = "Mazda RX-7 (Yellow)" });

            // Commands
            SelectVehicleCommand = new Command<BookingVehicleDto>(async vehicle =>
            {
                if (vehicle == null)
                    return;

                await Shell.Current.DisplayAlert("Vehicle Selected",
                    $"You chose {vehicle.VehicleDisplayName}", "OK");
            });

            GoHomeCommand = new Command(async () =>
                await Shell.Current.DisplayAlert("Navigation", "Go Home clicked", "OK"));
            GoAppointmentsCommand = new Command(async () =>
                await Shell.Current.DisplayAlert("Navigation", "Go Appointments clicked", "OK"));
            GoRewardsCommand = new Command(async () =>
                await Shell.Current.DisplayAlert("Navigation", "Go Rewards clicked", "OK"));
            GoAccountCommand = new Command(async () =>
                await Shell.Current.DisplayAlert("Navigation", "Go Account clicked", "OK"));
        }
    }
}
