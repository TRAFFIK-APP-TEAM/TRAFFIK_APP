namespace TRAFFIK_APP.Models.Dtos.Reward
{
    public class RedeemRewardRequest
    {
        public int UserId { get; set; }
        public int RewardId { get; set; }
    }
    
    public class RedeemCatalogItemRequest
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
    }

    public class RedemptionResponse
    {
        public int Redeemed { get; set; }
        public int ItemId { get; set; }
    }

    public class RedeemedRewardDto
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Cost { get; set; }
        public DateTime RedeemedAt { get; set; }
        public bool Used { get; set; }
    }
}