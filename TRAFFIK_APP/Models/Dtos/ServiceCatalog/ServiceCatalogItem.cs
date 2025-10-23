namespace TRAFFIK_APP.Models.Dtos.ServiceCatalog
{
    public class ServiceCatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? CarTypeId { get; set; }
        public string Category { get; set; } = string.Empty;
        public int EstimatedDurationMinutes { get; set; }
    }
}
