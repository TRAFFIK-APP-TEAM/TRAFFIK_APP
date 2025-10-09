using System;

public class ServiceHistory
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int ServiceCatalogId { get; set; }
    public DateTime DatePerformed { get; set; }
    public string Notes { get; set; } = string.Empty;

    public CarModel? Vehicle { get; set; }
    public ServiceCatalog? ServiceCatalog { get; set; }
}