using TRAFFIK_APP.Configuration;
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

        public Task<Booking?> CreateAsync(Booking booking) =>
            PostAsync<Booking>(Endpoints.Booking.Create, booking);

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