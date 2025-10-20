namespace TRAFFIK_APP.Models.Dtos.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; } // 1–5
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}