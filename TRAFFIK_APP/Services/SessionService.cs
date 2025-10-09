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
    }
}