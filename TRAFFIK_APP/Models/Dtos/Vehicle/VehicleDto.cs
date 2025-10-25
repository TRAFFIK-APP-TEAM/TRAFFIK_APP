namespace TRAFFIK_APP.Models.Dtos.Vehicle
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Make { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string LicensePlate { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty; // Base64 string

        public string VehicleType { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public int Year { get; set; }

        public DateTime? CreatedAt { get; set; } // Optional, set by backend if needed

        // Computed property for display
        public string DisplayName => $"{Make} {Model} ({LicensePlate})";
    }
}