using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Entities.Booking;
using TRAFFIK_APP.Models.Dtos.Booking;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class BookingClient : ApiClient
    {
        public BookingClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<Booking>?> GetAllAsync() =>
            GetAsync<List<Booking>>(Endpoints.Booking.GetAll);

        public Task<Booking?> GetByIdAsync(int id) =>
            GetAsync<Booking>(Endpoints.Booking.GetById.Replace("{id}", id.ToString()));

        public Task<Booking?> CreateAsync(Booking booking)
        {
            // Wrap the booking object to match API's expected JSON shape
            var payload = new { booking };
            return PostAsync<Booking>(Endpoints.Booking.Create, payload);
        }
        public Task<Booking?> CreateAsync(BookingCreateDto bookingDto) =>
            PostAsync<Booking>(Endpoints.Booking.Create, bookingDto);

        public Task<bool> UpdateAsync(int id, Booking booking) =>
            PutAsync(Endpoints.Booking.UpdateById.Replace("{id}", id.ToString()), booking);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.Booking.DeleteById.Replace("{id}", id.ToString()));

        public async Task<List<Booking>> GetByUserAsync(int userId)
        {
            var endpoint = Endpoints.Booking.GetByUser.Replace("{userId}", userId.ToString());
            return await GetAsync<List<Booking>>(endpoint);
        }
    }
}