namespace TRAFFIK_APP.Models.Dtos.User
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public string Permissions { get; set; } = string.Empty; // JSON string of permissions
    }
}