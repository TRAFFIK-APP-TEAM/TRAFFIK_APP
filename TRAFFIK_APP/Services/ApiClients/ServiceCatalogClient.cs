using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class ServiceCatalogClient : ApiClient
    {
        public ServiceCatalogClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<ServiceCatalog>?> GetAllAsync() =>
            GetAsync<List<ServiceCatalog>>(Endpoints.ServiceCatalog.GetAll);

        public Task<ServiceCatalog?> GetByIdAsync(int id) =>
            GetAsync<ServiceCatalog>(Endpoints.ServiceCatalog.GetById.Replace("{id}", id.ToString()));

        public Task<ServiceCatalog?> CreateAsync(ServiceCatalog service) =>
            PostAsync<ServiceCatalog>(Endpoints.ServiceCatalog.Create, service);

        public Task<bool> UpdateAsync(int id, ServiceCatalog service) =>
            PutAsync(Endpoints.ServiceCatalog.UpdateById.Replace("{id}", id.ToString()), service);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.ServiceCatalog.DeleteById.Replace("{id}", id.ToString()));
    }
}