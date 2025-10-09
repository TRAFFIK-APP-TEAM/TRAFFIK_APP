using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class CarModelClient : ApiClient
    {
        public CarModelClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<CarModel>?> GetAllAsync() =>
            GetAsync<List<CarModel>>(Endpoints.Vehicle.GetAll);

        public Task<CarModel?> GetByIdAsync(int id) =>
            GetAsync<CarModel>(Endpoints.Vehicle.GetById.Replace("{id}", id.ToString()));

        public Task<CarModel?> CreateAsync(CarModel model) =>
            PostAsync<CarModel>(Endpoints.Vehicle.Create, model);

        public Task<bool> UpdateAsync(int id, CarModel model) =>
            PutAsync(Endpoints.Vehicle.UpdateById.Replace("{id}", id.ToString()), model);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.Vehicle.DeleteById.Replace("{id}", id.ToString()));

        public Task<List<CarModel>?> GetByUserAsync(int userId) =>
            GetAsync<List<CarModel>>(Endpoints.Vehicle.GetByUser.Replace("{userId}", userId.ToString()));
    }
}