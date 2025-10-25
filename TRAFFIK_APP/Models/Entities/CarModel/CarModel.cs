namespace TRAFFIK_APP.Models.Entities.CarModel
{
    public class CarModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarTypeId { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
