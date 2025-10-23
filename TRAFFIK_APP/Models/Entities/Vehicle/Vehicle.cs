
namespace TRAFFIK_APP.Models.Entities.Vehicle
{
    public class Vehicle
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Computed property for display
    public string DisplayName => $"{Make} {Model} ({LicensePlate})";
    }
}
