public class BookingCreateDto
{
    public int UserId { get; set; }
    public int CarModelId { get; set; }
    public int ServiceCatalogId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Location { get; set; } = string.Empty;
}