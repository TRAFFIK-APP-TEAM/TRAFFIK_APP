namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public int ServiceCatalogId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        
        // Navigation properties for display
        public string UserName { get; set; } = string.Empty;
        public string VehicleDisplayName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
    }
}
