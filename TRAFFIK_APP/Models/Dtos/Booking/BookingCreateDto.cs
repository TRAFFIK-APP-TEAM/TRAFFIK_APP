namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingCreateDto
    {
        public int UserId { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public int ServiceCatalogId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}