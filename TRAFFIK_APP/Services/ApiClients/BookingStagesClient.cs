using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;
using TRAFFIK_APP.Models.Dtos.Booking;
using TRAFFIK_APP.Services;

namespace TRAFFIK_APP.Services.ApiClients
{
	public class BookingStagesClient : ApiClient
	{
		private readonly SessionService _sessionService;
		
		public BookingStagesClient(HttpClient httpClient, ILogger<ApiClient> logger, SessionService sessionService) 
			: base(httpClient, logger) 
		{ 
			_sessionService = sessionService;
		}

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

		public async Task<bool> UpdateStageAsync(BookingStageUpdateDto dto)
		{
			// Try the simple format that Azure API expects
			var requestPayload = new
			{
				BookingId = dto.BookingId,
				SelectedStage = dto.SelectedStage
			};
			
			return await PostAsync<bool>(Endpoints.BookingStages.UpdateStage, requestPayload);
		}
	}
}


