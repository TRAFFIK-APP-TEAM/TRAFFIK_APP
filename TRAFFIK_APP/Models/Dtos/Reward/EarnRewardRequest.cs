namespace TRAFFIK_APP.Models.Dtos.Reward {
    public class EarnRewardRequest
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public decimal AmountSpent { get; set; }
    }
}
