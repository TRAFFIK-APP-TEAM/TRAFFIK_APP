using TRAFFIK_APP.Models.Entities.User;

public class Reward
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Points { get; set; }
    public string Description { get; set; } = string.Empty;

    public User? User { get; set; }
}