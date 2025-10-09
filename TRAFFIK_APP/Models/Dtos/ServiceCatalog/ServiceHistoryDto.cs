public class ServiceHistoryDto
{
    public int VehicleId { get; set; }
    public int ServiceCatalogId { get; set; }
    public DateTime DatePerformed { get; set; }
    public string Notes { get; set; } = string.Empty;
}