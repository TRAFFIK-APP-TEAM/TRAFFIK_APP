using TRAFFIK_APP.Models.Entities.User;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CarModelId { get; set; }
    public int ServiceCatalogId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsConfirmed { get; set; }

    public User? User { get; set; }
    public ServiceCatalog? ServiceCatalog { get; set; }
}