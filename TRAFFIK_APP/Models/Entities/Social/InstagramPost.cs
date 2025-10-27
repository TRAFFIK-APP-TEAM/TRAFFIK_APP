namespace TRAFFIK_APP.Models.Entities.Social;

public class InstagramPost
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public DateTime PostedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public int Likes { get; set; } = 0;
    public int Comments { get; set; } = 0;
    public string Hashtags { get; set; } = string.Empty;
}