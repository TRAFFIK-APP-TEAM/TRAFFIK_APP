using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Entities.Reward;
using Microsoft.Extensions.Logging;
using TRAFFIK_APP.Models.Dtos.Reward;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class RewardClient : ApiClient
    {
        public RewardClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<Reward>?> GetAllAsync() =>
            GetAsync<List<Reward>>(Endpoints.Reward.GetAll);

        public Task<Reward?> GetByIdAsync(int id) =>
            GetAsync<Reward>(Endpoints.Reward.GetById.Replace("{id}", id.ToString()));

        public Task<Reward?> CreateAsync(Reward reward) =>
            PostAsync<Reward>(Endpoints.Reward.Create, reward);

        public Task<List<Reward>?> GetByUserAsync(int userId)
        {
            var endpoint = Endpoints.Reward.GetByUser.Replace("{userId}", userId.ToString());
            return GetAsync<List<Reward>>(endpoint);
        }

        public Task<int?> GetBalanceAsync(int userId)
        {
            var endpoint = Endpoints.Reward.GetBalance.Replace("{userId}", userId.ToString());
            return GetAsync<int?>(endpoint);
        }
    }
}