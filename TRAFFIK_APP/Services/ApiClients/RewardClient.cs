using TRAFFIK_APP.Configuration;
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

        public Task<bool> UpdateAsync(int id, Reward reward) =>
            PutAsync(Endpoints.Reward.UpdateById.Replace("{id}", id.ToString()), reward);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.Reward.DeleteById.Replace("{id}", id.ToString()));

        public async Task<int?> GetBalanceAsync(int userId)
        {
            var result = await GetAsync<int>(Endpoints.Reward.GetBalance.Replace("{userId}", userId.ToString()));
            return result;
        }

        public Task<Reward?> EarnAsync(EarnRewardRequest dto) =>
            PostAsync<Reward>(Endpoints.Reward.Earn, dto);

        public Task<bool> RedeemAsync(RedeemRewardRequest dto) =>
            PostAsync<bool>(Endpoints.Reward.Redeem, dto);
    }
}