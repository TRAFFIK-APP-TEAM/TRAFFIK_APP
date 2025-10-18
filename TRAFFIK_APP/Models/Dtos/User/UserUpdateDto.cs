public class UserUpdateDto
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public int RoleId { get; set; }
}