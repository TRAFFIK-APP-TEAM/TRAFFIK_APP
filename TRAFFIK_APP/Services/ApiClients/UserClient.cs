using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Entities.User;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class UserClient : ApiClient
    {
        public UserClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<User>?> GetAllAsync() =>
            GetAsync<List<User>>(Endpoints.User.GetAll);

        public Task<User?> GetByIdAsync(int id) =>
            GetAsync<User>(Endpoints.User.GetById.Replace("{id}", id.ToString()));

        public Task<User?> CreateAsync(User user) =>
            PostAsync<User>(Endpoints.User.Create, user);

        public Task<bool> UpdateAsync(int id, User user) =>
            PutAsync(Endpoints.User.UpdateById.Replace("{id}", id.ToString()), user);
        
        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.User.DeleteById.Replace("{id}", id.ToString()));
    }
}