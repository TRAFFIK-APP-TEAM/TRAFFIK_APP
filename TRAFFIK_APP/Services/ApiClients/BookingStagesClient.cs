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

		public Task<bool> UpdateStageAsync(BookingStageUpdateDto updateDto) =>
			PutAsync(Endpoints.BookingStages.UpdateStage, updateDto);
	}
}


