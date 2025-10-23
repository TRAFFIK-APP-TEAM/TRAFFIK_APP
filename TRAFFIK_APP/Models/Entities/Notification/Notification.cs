using TRAFFIK_APP.Models.Entities;

namespace TRAFFIK_APP.Models.Entities.Notification
{
    public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsRead { get; set; } = false;
    public string Type { get; set; } = "Info"; // Info, Warning, Success, Error
    public string Title { get; set; } = string.Empty;

    public User? User { get; set; }
    }
}