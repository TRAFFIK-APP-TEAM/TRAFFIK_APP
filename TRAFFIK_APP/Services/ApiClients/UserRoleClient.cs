using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class UserRoleClient : ApiClient
    {
        public UserRoleClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<UserRole>?> GetAllAsync() =>
            GetAsync<List<UserRole>>(Endpoints.UserRole.GetAll);

        public Task<UserRole?> GetByIdAsync(int id) =>
            GetAsync<UserRole>(Endpoints.UserRole.GetById.Replace("{id}", id.ToString()));

        public Task<UserRole?> CreateAsync(UserRole role) =>
            PostAsync<UserRole>(Endpoints.UserRole.Create, role);

        public Task<bool> UpdateAsync(int id, UserRole role) =>
            PutAsync(Endpoints.UserRole.UpdateById.Replace("{id}", id.ToString()), role);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.UserRole.DeleteById.Replace("{id}", id.ToString()));
    }
}