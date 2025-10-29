using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Services;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Views;


namespace TRAFFIK_APP.ViewModels
{
	public class StaffDashboardViewModel : BaseViewModel
	{
		private readonly SessionService _sessionService;
		private readonly BookingClient _bookingClient;

		public ObservableCollection<BookingDto> ActiveBookings { get; set; } = new();
		public int ActiveBookingsCount => ActiveBookings.Count;

		public ICommand EditProfileCommand { get; }
        public ICommand ViewAllBookingsCommand { get; }
        public ICommand ViewCodesCommand { get; }

        public StaffDashboardViewModel(SessionService sessionService, BookingClient bookingClient)
		{
			_sessionService = sessionService;
			_bookingClient = bookingClient;

			EditProfileCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(StaffProfilePage)));
			ViewAllBookingsCommand = new Command(async () =>
			{
				await Shell.Current.GoToAsync($"/StaffBookingListPage");
			});
			ViewCodesCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(StaffViewCodesPage)));

			// Observe collection changes to update count
			ActiveBookings.CollectionChanged += (_, __) =>
			{
				OnPropertyChanged(nameof(ActiveBookingsCount));
			};

			// Auto-load on create for staff only
			if (_sessionService.RoleId == 2)
			{
				_ = LoadActiveBookingsAsync();
			}
		}

		public async Task LoadActiveBookingsAsync()
		{
			ActiveBookings.Clear();
			var items = await _bookingClient.GetStaffBookingsAsync();
			if (items is null) return;
			
			// Filter for active bookings (not completed or closed/paid)
			foreach (var item in items)
			{
				// Status can be: "In Progress", "Completed", or "Closed" (Paid)
				if (item.Status != "Completed" && item.Status != "Closed" && item.Status != "Paid")
				{
					ActiveBookings.Add(item);
				}
			}
			
			OnPropertyChanged(nameof(ActiveBookingsCount));
		}
	}
}
