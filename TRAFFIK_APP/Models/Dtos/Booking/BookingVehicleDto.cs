namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingVehicleDto
    {
        public int UserId { get; set; }  // Associated user ID

        public int VehicleId { get; set; }  // Unique vehicle ID from the database

        public string VehicleDisplayName { get; set; } = string.Empty;  // Combined name for UI display (e.g. "Toyota Corolla")

        public string Make { get; set; } = string.Empty;  // Vehicle make (e.g. Toyota)

        public string Model { get; set; } = string.Empty;  // Vehicle model (e.g. Corolla)

        public string PlateNumber { get; set; } = string.Empty;  // License plate (e.g. CA 123-456)

        public string Color { get; set; } = string.Empty;  // Optional – for richer UI display

        public int Year { get; set; }  // Optional – to distinguish between multiple cars
    }
}
