namespace TRAFFIK_APP.Models.Dtos.Reward
{
    public class EarnRewardRequest
    {
        public int UserId { get; set; }
        public int BookingId { get; set; }
        public int Points { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}