using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Dtos.Auth;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class AuthClient : ApiClient
    {
        public AuthClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<UserLoginResponseDto?> LoginAsync(UserLoginDto dto) =>
            PostAsync<UserLoginResponseDto>(Endpoints.Auth.Login, dto);

        public Task<UserLoginResponseDto?> RegisterAsync(UserRegisterDto dto) =>
            PostAsync<UserLoginResponseDto>(Endpoints.Auth.Register, dto);

        public Task<bool> LogoutAsync() =>
            PostAsync<bool>(Endpoints.Auth.Logout, new { });

        public Task<bool> DeleteAccountAsync(int id) =>
            DeleteAsync(Endpoints.Auth.DeleteAccount.Replace("{id}", id.ToString()));
    }
}