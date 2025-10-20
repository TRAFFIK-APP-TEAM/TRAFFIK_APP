using System;

public class ServiceHistory
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int ServiceCatalogId { get; set; }
    public DateTime DatePerformed { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string PerformedBy { get; set; } = string.Empty; // Staff member who performed the service
    public decimal Cost { get; set; }
    public string Status { get; set; } = "Completed"; // Completed, In Progress, Cancelled
    
    public ServiceCatalog? ServiceCatalog { get; set; }
}