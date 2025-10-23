using TRAFFIK_APP.Models.Entities;

namespace TRAFFIK_APP.Models.Entities.Reward
{
    public class Reward
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Points { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime EarnedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsRedeemed { get; set; } = false;
    public DateTime? RedeemedAt { get; set; }

    public User? User { get; set; }
    }
}