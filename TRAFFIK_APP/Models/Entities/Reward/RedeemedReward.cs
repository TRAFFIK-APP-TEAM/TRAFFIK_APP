using TRAFFIK_APP.Models.Entities;

namespace TRAFFIK_APP.Models.Entities.Reward
{
    public class RedeemedReward
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; } // FK to RewardItem
        public string Code { get; set; } = string.Empty;
        public bool Used { get; set; } = false;
        public DateTime RedeemedAt { get; set; }

        // Navigation properties
        public User? User { get; set; }
        public RewardItem? RewardItem { get; set; }
    }
}

