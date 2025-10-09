public class CarTypeServices
{
    public int Id { get; set; }
    public int CarTypeId { get; set; }
    public int ServiceCatalogId { get; set; }

    public CarType? CarType { get; set; }
    public ServiceCatalog? ServiceCatalog { get; set; }
}