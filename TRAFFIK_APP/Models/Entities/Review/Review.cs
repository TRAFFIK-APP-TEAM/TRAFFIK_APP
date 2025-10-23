using TRAFFIK_APP.Models.Entities;

namespace TRAFFIK_APP.Models.Entities.Review
{
    public class Review
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; } // 1–5
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsVerified { get; set; } = false;

    public TRAFFIK_APP.Models.Entities.Booking.Booking? Booking { get; set; }
    public User? User { get; set; }
    }
}