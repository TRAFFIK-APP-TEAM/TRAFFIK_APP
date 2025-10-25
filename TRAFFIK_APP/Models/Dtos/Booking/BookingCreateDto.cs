namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingCreateDto
    {
        public int UserId { get; set; }
        public int? ServiceId { get; set; }
        public int CarModelId { get; set; }
        public int? ServiceCatalogId { get; set; }
        public string VehicleLicensePlate { get; set; } = string.Empty;
        public DateOnly BookingDate { get; set; }
        public TimeOnly BookingTime { get; set; }
        public string Status { get; set; } = "Pending";
    }
}