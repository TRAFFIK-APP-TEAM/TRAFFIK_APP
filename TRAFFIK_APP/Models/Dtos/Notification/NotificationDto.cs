namespace TRAFFIK_APP.Models.Dtos.Notification
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; } = false;
        public string Type { get; set; } = "Info"; // Info, Warning, Success, Error
        public string Title { get; set; } = string.Empty;
    }
}