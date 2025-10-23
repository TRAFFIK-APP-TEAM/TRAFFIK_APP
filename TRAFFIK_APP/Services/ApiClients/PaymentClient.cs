using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class PaymentClient : ApiClient
    {
        public PaymentClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<Payment>?> GetAllAsync() =>
            GetAsync<List<Payment>>(Endpoints.Payment.GetAll);

        public Task<Payment?> GetByIdAsync(int id) =>
            GetAsync<Payment>(Endpoints.Payment.GetById.Replace("{id}", id.ToString()));

        public Task<Payment?> CreateAsync(Payment payment) =>
            PostAsync<Payment>(Endpoints.Payment.Create, payment);

        public Task<bool> UpdateAsync(int id, Payment payment) =>
            PutAsync(Endpoints.Payment.UpdateById.Replace("{id}", id.ToString()), payment);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.Payment.DeleteById.Replace("{id}", id.ToString()));
    }
}