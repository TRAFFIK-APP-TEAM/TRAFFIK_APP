using TRAFFIK_APP.Models.Dtos;
using TRAFFIK_APP.Services.ApiClients;
using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;
public class RewardCatalogClient : ApiClient
{
    public RewardCatalogClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

    public Task<List<RewardItemDto>?> GetAllAsync() =>
        GetAsync<List<RewardItemDto>>(Endpoints.RewardCatalog.GetAll);

    public Task<bool> RedeemItemAsync(int itemId, int userId)
    {
        var dto = new RedeemCatalogItemRequest
        {
            UserId = userId,
            ItemId = itemId
        };
        return PostAsync<bool>(Endpoints.RewardCatalog.RedeemItem.Replace("{itemId}", itemId.ToString()), dto);
    }
}