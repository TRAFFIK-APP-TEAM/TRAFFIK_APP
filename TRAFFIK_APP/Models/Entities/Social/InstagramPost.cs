public class InstagramPost
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public DateTime PostedAt { get; set; }
}