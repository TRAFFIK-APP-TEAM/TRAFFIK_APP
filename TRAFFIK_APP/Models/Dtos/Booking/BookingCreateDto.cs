namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingCreateDto
    {
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public int ServiceCatalogId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}