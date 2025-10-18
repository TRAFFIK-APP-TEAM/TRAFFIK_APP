namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingVehicleDto
    {
        public int UserId { get; set; }      // Unique ID for the vehicle / user
        public string VehicleDisplayName { get; set; } = string.Empty;  // Name to show in UI
    }
}
