namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingServiceDto
    {
        public int ServiceCatalogId { get; set; }  // Unique ID of the service from catalog

        public string ServiceName { get; set; } = string.Empty;  // Name shown in UI (e.g. "Full Car Wash")

        public string Description { get; set; } = string.Empty;  // Optional description (e.g. "Exterior + Interior clean")

        public decimal Price { get; set; }  // Service price

        public int EstimatedDurationMinutes { get; set; }  // Estimated duration for appointment (e.g. 45 mins)

        public string Category { get; set; } = string.Empty;  // Optional: category name (e.g. "Wash", "Detailing", etc.)

        public bool IsAvailable { get; set; } = true;  // Used for filtering or disabling unavailable services
    }
}
