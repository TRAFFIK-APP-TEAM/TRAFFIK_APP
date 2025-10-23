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
}