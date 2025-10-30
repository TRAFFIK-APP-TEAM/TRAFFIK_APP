using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using System.Windows.Input;
using System.Collections.ObjectModel;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Views;

namespace TRAFFIK_APP.ViewModels
{
    public class BookingViewModel : BaseViewModel
    {
        private readonly SessionService _session;
        private readonly BookingClient _bookingClient;

        public string UserFullName => _session.UserName;
        public ObservableCollection<BookingDto> Bookings { get; } = new();
        public ObservableCollection<BookingDto> ClosedBookings { get; } = new();
        public bool HasBookings => Bookings.Count > 0;
        public bool NoBookings => Bookings.Count == 0;
        public bool HasClosedBookings => ClosedBookings.Count > 0;
        public bool NoClosedBookings => ClosedBookings.Count == 0;

        public ICommand GoHomeCommand { get; }
        public ICommand GoAppointmentsCommand { get; }
        public ICommand GoRewardsCommand { get; }
        public ICommand GoAccountCommand { get; }
        public ICommand EditProfileCommand { get; }
        public ICommand StartBookingCommand { get; }
        public ICommand LoadBookingsCommand { get; }
        public ICommand ViewBookingDetailsCommand { get; }
        public ICommand ShowHistoryCommand { get; }

        public BookingViewModel(SessionService session, BookingClient bookingClient)
        {
            _session = session;
            _bookingClient = bookingClient;

            GoHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//DashboardPage"));
            GoAppointmentsCommand = new Command(async () => await Shell.Current.GoToAsync("//BookingPage"));
            GoRewardsCommand = new Command(async () => await Shell.Current.GoToAsync("//RewardsPage"));
            GoAccountCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
            EditProfileCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
            StartBookingCommand = new Command(async () => await Shell.Current.GoToAsync($"/BookingVehicleSelectPage"));
            LoadBookingsCommand = new Command(() => ExecuteSafeAsync(LoadBookingsAsync, "Loading bookings..."));
            ViewBookingDetailsCommand = new Command<int>(async (bookingId) =>
            {
                // Store the selected booking ID and navigate to detail page
                // Check both active and closed bookings
                var booking = Bookings.FirstOrDefault(b => b.Id == bookingId) 
                    ?? ClosedBookings.FirstOrDefault(b => b.Id == bookingId);
                if (booking != null)
                {
                    BookingDetailPage.SelectedBooking = booking;
                    await Shell.Current.GoToAsync(nameof(BookingDetailPage));
                }
            });
            ShowHistoryCommand = new Command(async () => await ShowHistoryAsync());

            // Observe collection changes to update HasBookings/NoBookings
            Bookings.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(HasBookings));
                OnPropertyChanged(nameof(NoBookings));
                OnPropertyChanged(nameof(HasClosedBookings));
            };

            ClosedBookings.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(HasClosedBookings));
                OnPropertyChanged(nameof(NoClosedBookings));
            };
        }

        public async Task LoadBookingsAsync()
        {
            if (_session.UserId is not int userId)
            {
                ErrorMessage = "Session expired. Please log in again.";
                await Shell.Current.GoToAsync(nameof(LoginPage));
                return;
            }

            Bookings.Clear();
            ClosedBookings.Clear();
            var bookings = await _bookingClient.GetByUserAsync(userId);
            
            System.Diagnostics.Debug.WriteLine($"[BookingViewModel] Loaded {bookings?.Count ?? 0} bookings");
            
            if (bookings is not null)
            {
                foreach (var booking in bookings)
                {
                    System.Diagnostics.Debug.WriteLine($"[BookingViewModel] Booking: Id={booking.Id}, ServiceCatalogId={booking.ServiceCatalogId}, ServiceName='{booking.ServiceName}', VehiclePlate='{booking.VehicleLicensePlate}', VehicleName='{booking.VehicleDisplayName}', Date={booking.BookingDate}, Time={booking.BookingTime}, Status={booking.Status}");
                    
                    // Separate active and closed bookings
                    // A booking is closed if status is "Paid" or "Closed"
                    if (booking.Status == "Paid" || booking.Status == "Closed")
                    {
                        ClosedBookings.Add(booking);
                    }
                    else
                    {
                        Bookings.Add(booking);
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[BookingViewModel] Bookings is null!");
            }
            
            OnPropertyChanged(nameof(HasBookings));
            OnPropertyChanged(nameof(NoBookings));
            OnPropertyChanged(nameof(HasClosedBookings));
            OnPropertyChanged(nameof(NoClosedBookings));
        }

        private async Task ShowHistoryAsync()
        {
            if (ClosedBookings.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("History", "You have no closed bookings yet.", "OK");
                return;
            }

            // Navigate to booking history page
            await Shell.Current.GoToAsync(nameof(BookingHistoryPage));
        }
    }
}

