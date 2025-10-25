using System.Text.Json.Serialization;
using TRAFFIK_APP.Models.Entities;
using TRAFFIK_APP.Models.Entities.Vehicle;
namespace TRAFFIK_APP.Models.Entities.Booking
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int CarModelId { get; set; }
        public int? ServiceCatalogId { get; set; }
        public string VehicleLicensePlate { get; set; } = string.Empty;
        public TimeOnly BookingTime { get; set; }
        public DateOnly BookingDate { get; set; }
        public string Status { get; set; } = "Pending";

        [JsonIgnore] public User? User { get; set; }
        [JsonIgnore] public TRAFFIK_APP.Models.Entities.Vehicle.Vehicle? Vehicle { get; set; }
        [JsonIgnore] public TRAFFIK_APP.Models.Entities.ServiceCatalog.ServiceCatalog? ServiceCatalog { get; set; }
    }
}
