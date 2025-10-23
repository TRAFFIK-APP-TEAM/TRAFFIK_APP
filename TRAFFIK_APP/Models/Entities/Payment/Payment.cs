using TRAFFIK_APP.Models.Entities.Booking;
public class Payment
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string TransactionId { get; set; } = string.Empty;

    public Booking? Booking { get; set; }
}