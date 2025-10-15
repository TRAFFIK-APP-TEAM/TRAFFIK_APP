using TRAFFIK_APP.Models.Dtos.Auth;

namespace TRAFFIK_APP.Models.Entities.User
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int RoleId { get; set; }

        // Create User from login response
        public static User FromLoginResponse(UserLoginResponseDto response)
        {
            return new User
            {
                Id = response.Id,
                FullName = response.FullName,
                Email = response.Email,
                PhoneNumber = response.PhoneNumber,
                RoleId = response.RoleId
            };
        }
    }
}