namespace TRAFFIK_APP.Models.Dtos.Booking
{
    public class BookingServiceDto
    {
        public int Id { get; set; }
        public int ServiceCatalogId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
