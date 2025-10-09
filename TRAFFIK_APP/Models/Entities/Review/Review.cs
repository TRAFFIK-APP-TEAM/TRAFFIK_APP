using TRAFFIK_APP.Models.Entities.User;

public class Review
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; } // 1–5
    public string Comment { get; set; } = string.Empty;

    public Booking? Booking { get; set; }
    public User? User { get; set; }
}