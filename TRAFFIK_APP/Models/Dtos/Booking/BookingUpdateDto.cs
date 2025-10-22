namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingUpdateDto
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
        public string Status { get; set; } = "Pending";
    }
}