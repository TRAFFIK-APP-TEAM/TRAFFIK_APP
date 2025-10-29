using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;
using Microsoft.Maui.Controls;

namespace TRAFFIK_APP.ViewModels
{
    public class AdminManageBookingsViewModel : BaseViewModel
    {
        private readonly BookingClient _bookingClient;

        public ObservableCollection<BookingDto> AllBookings { get; } = new();

        public ICommand LoadBookingsCommand { get; }
        public ICommand ViewBookingCommand { get; }
        public ICommand DeleteBookingCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand GoBackCommand { get; }

        public bool HasBookings => AllBookings.Count > 0;
        public bool NoBookings => AllBookings.Count == 0;

        public AdminManageBookingsViewModel(BookingClient bookingClient)
        {
            _bookingClient = bookingClient;

            LoadBookingsCommand = new Command(() => ExecuteSafeAsync(LoadAllBookingsAsync, "Loading bookings..."));
            RefreshCommand = new Command(() => ExecuteSafeAsync(LoadAllBookingsAsync, "Refreshing..."));
            GoBackCommand = new Command(async () => 
            {
                // Navigate back to AdminDashboardPage using absolute path
                await Shell.Current.GoToAsync("//AdminDashboardPage");
            });
            
            ViewBookingCommand = new Command<BookingDto>(async (booking) =>
            {
                if (booking != null)
                {
                    // Store the selected booking and navigate to detail page
                    BookingDetailPage.SelectedBooking = booking;
                    BookingDetailPage.SourcePage = "AdminManageBookingsPage";
                    await Shell.Current.GoToAsync("BookingDetailPage");
                }
            });

            DeleteBookingCommand = new Command<BookingDto>(async (booking) =>
            {
                if (booking != null)
                {
                    var confirm = await Application.Current.MainPage.DisplayAlert(
                        "Delete Booking",
                        $"Are you sure you want to delete booking #{booking.Id}? This action cannot be undone.",
                        "Delete",
                        "Cancel");

                    if (confirm)
                    {
                        await DeleteBookingAsync(booking.Id);
                    }
                }
            });

            // Observe collection changes
            AllBookings.CollectionChanged += (_, __) =>
            {
                OnPropertyChanged(nameof(HasBookings));
                OnPropertyChanged(nameof(NoBookings));
            };

            // Load bookings when view model is created
            LoadBookingsCommand.Execute(null);
        }

        private async Task LoadAllBookingsAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[AdminManageBookingsViewModel] Starting to load all bookings...");
                var bookings = await _bookingClient.GetAllAsync();
                
                System.Diagnostics.Debug.WriteLine($"[AdminManageBookingsViewModel] API returned {bookings?.Count ?? 0} bookings");

                AllBookings.Clear();
                
                if (bookings != null && bookings.Count > 0)
                {
                    foreach (var booking in bookings)
                    {
                        System.Diagnostics.Debug.WriteLine($"[AdminManageBookingsViewModel] Adding booking: Id={booking.Id}, ServiceName='{booking.ServiceName}', Status='{booking.Status}', Vehicle='{booking.VehicleLicensePlate}', Date={booking.BookingDate}");
                        AllBookings.Add(booking);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[AdminManageBookingsViewModel] No bookings found");
                }
                
                System.Diagnostics.Debug.WriteLine($"[AdminManageBookingsViewModel] AllBookings collection now has {AllBookings.Count} items");
                OnPropertyChanged(nameof(AllBookings));
                OnPropertyChanged(nameof(HasBookings));
                OnPropertyChanged(nameof(NoBookings));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AdminManageBookingsViewModel] Error loading bookings: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[AdminManageBookingsViewModel] Stack trace: {ex.StackTrace}");
                ErrorMessage = $"Error loading bookings: {ex.Message}";
            }
        }

        private async Task DeleteBookingAsync(int bookingId)
        {
            try
            {
                IsBusy = true;
                System.Diagnostics.Debug.WriteLine($"[AdminManageBookingsViewModel] Deleting booking {bookingId}");
                
                var success = await _bookingClient.DeleteAsync(bookingId);
                
                if (success)
                {
                    // Remove from collection
                    var booking = AllBookings.FirstOrDefault(b => b.Id == bookingId);
                    if (booking != null)
                    {
                        AllBookings.Remove(booking);
                    }
                    
                    await Application.Current.MainPage.DisplayAlert("Success", "Booking deleted successfully.", "OK");
                }
                else
                {
                    ErrorMessage = "Failed to delete booking. Please try again.";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AdminManageBookingsViewModel] Error deleting booking: {ex.Message}");
                ErrorMessage = $"Error deleting booking: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

