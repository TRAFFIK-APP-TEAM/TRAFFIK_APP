using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;


namespace TRAFFIK_APP.ViewModels
{
    public class StaffBookingListViewModel : BaseViewModel
    {
        private readonly BookingClient _bookingClient;
        private DateOnly _selectedDate;

        public DateOnly SelectedDate 
        { 
            get => _selectedDate;
            set 
            {
                SetProperty(ref _selectedDate, value);
                FilterBookingsByDate();
            }
        }

        public ObservableCollection<BookingDto> AllBookings { get; } = new();
        public ICommand ViewBookingCommand { get; }
        public ICommand LoadBookingsCommand { get; }
        public ICommand GoBackCommand { get; }

        public StaffBookingListViewModel(BookingClient bookingClient)
        {
            _bookingClient = bookingClient;
            _selectedDate = DateOnly.FromDateTime(DateTime.Now); // Default to today

            ViewBookingCommand = new Command<BookingDto>(async (booking) =>
            {
                if (booking != null)
                {
                    // Convert BookingDto to BookingStageUpdateDto for the detail page
                    var stageDto = new BookingStageUpdateDto
                    {
                        Id = booking.Id,
                        BookingId = booking.Id,
                        CurrentStage = booking.Status,
                        VehicleId = 0, // Not used
                        AvailableStages = new List<string>
                        {
                            "Service Started", "Service Completed", "Service Paid"
                        }
                    };
                    StaffBookingDetailPage.SelectedBooking = stageDto;
                    await Shell.Current.GoToAsync("StaffBookingDetailPage");
                }
            });

            LoadBookingsCommand = new Command(() => ExecuteSafeAsync(LoadAllBookingsAsync, "Loading bookings..."));
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            
            // Observe collection changes
            AllBookings.CollectionChanged += (_, __) =>
            {
                System.Diagnostics.Debug.WriteLine($"[StaffBookingListViewModel] Collection changed. Count: {AllBookings.Count}");
                OnPropertyChanged(nameof(HasBookings));
                OnPropertyChanged(nameof(NoBookings));
            };
            
            // Load bookings when view model is created
            LoadBookingsCommand.Execute(null);
        }

        public async Task LoadAllBookingsAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[StaffBookingListViewModel] Starting to load bookings...");
                var bookings = await _bookingClient.GetStaffBookingsAsync();
                
                System.Diagnostics.Debug.WriteLine($"[StaffBookingListViewModel] API returned {bookings?.Count ?? 0} bookings");
                
                if (bookings is null)
                {
                    System.Diagnostics.Debug.WriteLine("[StaffBookingListViewModel] Bookings is NULL!");
                    ErrorMessage = "No bookings data received from server";
                    return;
                }

                if (bookings.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("[StaffBookingListViewModel] No bookings found in the list");
                    ErrorMessage = "No bookings found";
                    return;
                }

                AllBookings.Clear();
                _allBookings = bookings;
                FilterBookingsByDate();
                
                System.Diagnostics.Debug.WriteLine($"[StaffBookingListViewModel] AllBookings collection now has {AllBookings.Count} items");
                OnPropertyChanged(nameof(AllBookings));
                OnPropertyChanged(nameof(HasBookings));
                OnPropertyChanged(nameof(NoBookings));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[StaffBookingListViewModel] Error loading bookings: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[StaffBookingListViewModel] Stack trace: {ex.StackTrace}");
                ErrorMessage = $"Error loading bookings: {ex.Message}";
            }
        }

        public bool HasBookings => AllBookings.Count > 0;
        public bool NoBookings => AllBookings.Count == 0;

        private List<BookingDto> _allBookings = new();

        private void FilterBookingsByDate()
        {
            if (_allBookings == null || _allBookings.Count == 0)
                return;

            AllBookings.Clear();

            foreach (var booking in _allBookings)
            {
                // Skip closed/paid bookings
                if (booking.Status == "Closed" || booking.Status == "Paid")
                {
                    continue;
                }

                // Show bookings for the selected date
                if (booking.BookingDate == SelectedDate)
                {
                    AllBookings.Add(booking);
                }
            }

            OnPropertyChanged(nameof(AllBookings));
            OnPropertyChanged(nameof(HasBookings));
            OnPropertyChanged(nameof(NoBookings));
        }
    }
}
