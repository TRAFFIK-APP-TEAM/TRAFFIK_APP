using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;
using TRAFFIK_APP.Models.Dtos.ServiceCatalog;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class ServiceHistoryClient : ApiClient
    {
        public ServiceHistoryClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<ServiceHistory>?> GetAllAsync() =>
            GetAsync<List<ServiceHistory>>(Endpoints.ServiceHistory.GetAll);

        public Task<List<ServiceHistory>?> GetByVehicleAsync(int vehicleId) =>
            GetAsync<List<ServiceHistory>>(Endpoints.ServiceHistory.GetByVehicle.Replace("{vehicleId}", vehicleId.ToString()));

        public Task<bool> TrackWashAsync(ServiceHistoryDto dto) =>
            PostAsync<bool>(Endpoints.ServiceHistory.TrackWash, dto);
    }
}