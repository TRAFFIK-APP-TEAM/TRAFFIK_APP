using TRAFFIK_APP.Models.Dtos.Reward;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class RewardCatalogClient : ApiClient
    {
        public RewardCatalogClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<RewardItemDto>?> GetAllAsync() =>
            GetAsync<List<RewardItemDto>>(Endpoints.RewardCatalog.GetAll);

        public Task<RedemptionResponse> RedeemItemAsync(int itemId, int userId)
        {
            var dto = new RedeemCatalogItemRequest
            {
                UserId = userId,
                ItemId = itemId
            };

            var endpoint = Endpoints.RewardCatalog.RedeemItem.Replace("{itemId}", itemId.ToString());
            return PostAsync<RedemptionResponse>(endpoint, dto);
        }

        public Task<List<RedeemedRewardDto>?> GetRedeemedAsync(int userId)
        {
            var endpoint = Endpoints.RewardCatalog.GetRedeemed.Replace("{userId}", userId.ToString());
            return GetAsync<List<RedeemedRewardDto>>(endpoint);
        }
    }
}