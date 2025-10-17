using TRAFFIK_APP.Services;
using System.Windows.Input;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingViewModel : BaseViewModel
    {
        private readonly SessionService _session;

        public string UserFullName => _session.UserName;

        public ICommand GoHomeCommand { get; }
        public ICommand GoAppointmentsCommand { get; }
        public ICommand GoRewardsCommand { get; }
        public ICommand GoAccountCommand { get; }
        public ICommand EditProfileCommand { get; }
        public ICommand StartBookingCommand { get; }

        public BookingViewModel(SessionService session)
        {
            _session = session;

            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
            EditProfileCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
            StartBookingCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(TRAFFIK_APP.Views.BookingServiceSelectPage)));
        }
    }
}

