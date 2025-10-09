using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class CarTypeServicesClient : ApiClient
    {
        public CarTypeServicesClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<CarTypeServices>?> GetAllAsync() =>
            GetAsync<List<CarTypeServices>>(Endpoints.CarTypeServices.GetAll);

        public Task<CarTypeServices?> GetByIdAsync(int id) =>
            GetAsync<CarTypeServices>(Endpoints.CarTypeServices.GetById.Replace("{id}", id.ToString()));

        public Task<CarTypeServices?> CreateAsync(CarTypeServices mapping) =>
            PostAsync<CarTypeServices>(Endpoints.CarTypeServices.Create, mapping);

        public Task<bool> UpdateAsync(int id, CarTypeServices mapping) =>
            PutAsync(Endpoints.CarTypeServices.UpdateById.Replace("{id}", id.ToString()), mapping);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.CarTypeServices.DeleteById.Replace("{id}", id.ToString()));
    }
}