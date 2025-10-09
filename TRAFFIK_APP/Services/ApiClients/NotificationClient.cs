using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class NotificationClient : ApiClient
    {
        public NotificationClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<Notification>?> GetAllAsync() =>
            GetAsync<List<Notification>>(Endpoints.Notification.GetAll);

        public Task<Notification?> GetByIdAsync(int id) =>
            GetAsync<Notification>(Endpoints.Notification.GetById.Replace("{id}", id.ToString()));

        public Task<Notification?> CreateAsync(Notification notification) =>
            PostAsync<Notification>(Endpoints.Notification.Create, notification);

        public Task<bool> UpdateAsync(int id, Notification notification) =>
            PutAsync(Endpoints.Notification.UpdateById.Replace("{id}", id.ToString()), notification);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.Notification.DeleteById.Replace("{id}", id.ToString()));
    }
}