namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int? ServiceCatalogId { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly BookingTime { get; set; }
        public string Status { get; set; } = "Pending";
        
        // Navigation properties for display
        public string UserName { get; set; } = string.Empty;
        public string VehicleDisplayName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
    }
}
