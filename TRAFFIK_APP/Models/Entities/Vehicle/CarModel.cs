using TRAFFIK_APP.Models.Entities.User;
public class CarModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int CarTypeId { get; set; }

    public User? User { get; set; }
    public CarType? CarType { get; set; }
}