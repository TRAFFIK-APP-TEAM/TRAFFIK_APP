namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingVehicleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }      // Unique ID for the vehicle / user
        public string VehicleDisplayName { get; set; } = string.Empty;  // Name to show in UI
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
    }
}
