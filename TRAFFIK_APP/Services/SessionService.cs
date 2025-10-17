using TRAFFIK_APP.Models.Entities.User;

namespace TRAFFIK_APP.Services
{
    public class SessionService
    {
        public User? CurrentUser { get; private set; }

        public bool IsLoggedIn => CurrentUser is not null;

        public void SetUser(User user)
        {
            CurrentUser = user;
        }

        public void Clear()
        {
            CurrentUser = null;
        }

        public int? UserId => CurrentUser?.Id;
        public string UserName => CurrentUser?.FullName ?? string.Empty;
        public int? RoleId => CurrentUser?.RoleId;

        public bool IsAdmin => RoleId == 1;
        public bool IsStaff => RoleId == 2;
        public bool IsUser => RoleId == 3;
    }
}