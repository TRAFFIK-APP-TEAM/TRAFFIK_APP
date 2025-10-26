using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;
using TRAFFIK_APP.Models.Entities.Social;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class InstagramPostsClient : ApiClient
    {
        public InstagramPostsClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<InstagramPost>?> GetAllAsync() =>
            GetAsync<List<InstagramPost>>(Endpoints.InstagramPosts.GetAll);

        public Task<InstagramPost?> GetByIdAsync(int id) =>
            GetAsync<InstagramPost>(Endpoints.InstagramPosts.GetById.Replace("{id}", id.ToString()));

        public Task<InstagramPost?> CreateAsync(InstagramPost post) =>
            PostAsync<InstagramPost>(Endpoints.InstagramPosts.Create, post);
    }
}

