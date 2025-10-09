using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class SocialFeedClient : ApiClient
    {
        public SocialFeedClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<InstagramPost>?> GetInstagramPostsAsync() =>
            GetAsync<List<InstagramPost>>(Endpoints.SocialFeed.GetInstagramPosts);
    }
}