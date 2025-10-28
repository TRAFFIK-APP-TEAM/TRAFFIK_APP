using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;
using TRAFFIK_APP.Models.Dtos.Booking;

namespace TRAFFIK_APP.Services.ApiClients
{
	public class BookingStagesClient : ApiClient
	{
		public BookingStagesClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

		public Task<List<BookingStageUpdateDto>?> GetAllAsync() =>
			GetAsync<List<BookingStageUpdateDto>>(Endpoints.BookingStages.GetAll);

		public Task<BookingStageUpdateDto?> GetByIdAsync(int id) =>
			GetAsync<BookingStageUpdateDto>(Endpoints.BookingStages.GetById.Replace("{id}", id.ToString()));

		public Task<BookingStageUpdateDto?> CreateAsync(BookingStageUpdateDto dto) =>
			PostAsync<BookingStageUpdateDto>(Endpoints.BookingStages.Create, dto);

		public Task<List<BookingStageUpdateDto>?> GetByBookingAsync(int bookingId)
		{
			var endpoint = Endpoints.BookingStages.GetByBooking.Replace("{bookingId}", bookingId.ToString());
			return GetAsync<List<BookingStageUpdateDto>>(endpoint);
		}

		public Task<bool> UpdateStageAsync(BookingStageUpdateDto dto) =>
			PostAsync<bool>(Endpoints.BookingStages.UpdateStage, dto);
	}
}


