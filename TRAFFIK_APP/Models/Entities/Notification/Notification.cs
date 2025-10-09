using TRAFFIK_APP.Models.Entities.User;

public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public User? User { get; set; }
}