using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;
using TRAFFIK_APP.Models.Dtos.ServiceCatalog;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class ServiceCatalogClient : ApiClient
    {
        public ServiceCatalogClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<ServiceCatalogDto>?> GetAllAsync() =>
            GetAsync<List<ServiceCatalogDto>>(Endpoints.ServiceCatalog.GetAll);

        public Task<ServiceCatalogDto?> GetByIdAsync(int id) =>
            GetAsync<ServiceCatalogDto>(Endpoints.ServiceCatalog.GetById.Replace("{id}", id.ToString()));

        public Task<ServiceCatalogDto?> CreateAsync(ServiceCatalogDto service) =>
            PostAsync<ServiceCatalogDto>(Endpoints.ServiceCatalog.Create, service);

        public Task<bool> UpdateAsync(int id, ServiceCatalogDto service) =>
            PutAsync(Endpoints.ServiceCatalog.UpdateById.Replace("{id}", id.ToString()), service);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.ServiceCatalog.DeleteById.Replace("{id}", id.ToString()));

        public Task<List<ServiceCatalogDto>?> GetForVehicleAsync(string licensePlate)
        {
            var endpoint = Endpoints.ServiceCatalog.GetForVehicle.Replace("{licensePlate}", licensePlate);
            return GetAsync<List<ServiceCatalogDto>>(endpoint);
        }

        public Task<List<ServiceCatalogDto>?> GetByVehicleTypeAsync(int vehicleTypeId)
        {
            var endpoint = Endpoints.ServiceCatalog.GetByVehicleType.Replace("{vehicleTypeId}", vehicleTypeId.ToString());
            return GetAsync<List<ServiceCatalogDto>>(endpoint);
        }
    }
}