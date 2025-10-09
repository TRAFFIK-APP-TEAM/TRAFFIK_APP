using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class CarTypeClient : ApiClient
    {
        public CarTypeClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<CarType>?> GetAllAsync() =>
            GetAsync<List<CarType>>(Endpoints.CarTypes.GetAll);

        public Task<CarType?> GetByIdAsync(int id) =>
            GetAsync<CarType>(Endpoints.CarTypes.GetById.Replace("{id}", id.ToString()));

        public Task<CarType?> CreateAsync(CarType type) =>
            PostAsync<CarType>(Endpoints.CarTypes.Create, type);

        public Task<bool> UpdateAsync(int id, CarType type) =>
            PutAsync(Endpoints.CarTypes.UpdateById.Replace("{id}", id.ToString()), type);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.CarTypes.DeleteById.Replace("{id}", id.ToString()));
    }
}