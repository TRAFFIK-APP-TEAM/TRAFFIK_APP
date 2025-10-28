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
		private readonly BookingStagesClient _bookingStagesClient;

		public ObservableCollection<BookingStageUpdateDto> ActiveBookings { get; set; } = new();

		public ICommand EditProfileCommand { get; }
        public ICommand ViewAllBookingsCommand { get; }

        public StaffDashboardViewModel(SessionService sessionService, BookingStagesClient bookingStagesClient)
		{
			_sessionService = sessionService;
			_bookingStagesClient = bookingStagesClient;

			EditProfileCommand = new Command(async () => await Shell.Current.GoToAsync("//AccountPage"));
			ViewAllBookingsCommand = new Command(async () =>
			{
				await Shell.Current.GoToAsync(nameof(StaffBookingListPage));
			});

			// Auto-load on create for staff only
			if (_sessionService.RoleId == 2)
			{
				_ = LoadActiveBookingsAsync();
			}
		}

		private async Task LoadActiveBookingsAsync()
		{
			ActiveBookings.Clear();
			var items = await _bookingStagesClient.GetAllAsync();
			if (items is null) return;
			foreach (var item in items)
			{
				ActiveBookings.Add(item);
			}
		}
	}
}
