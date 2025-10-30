using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Dtos.Auth;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class AuthClient : ApiClient
    {
        public AuthClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<UserLoginResponseDto?> LoginAsync(UserLoginDto dto) =>
            PostAsync<UserLoginResponseDto>(Endpoints.Auth.Login, dto);

        public Task<UserLoginResponseDto?> RegisterAsync(UserRegisterDto dto) =>
            PostAsync<UserLoginResponseDto>(Endpoints.Auth.Register, dto);

        public async Task<(UserLoginResponseDto? Result, string? ErrorMessage)> RegisterAsyncWithError(UserRegisterDto dto)
        {
            try
            {
                var url = Endpoints.Auth.Register;
                var fullUrl = _httpClient.BaseAddress != null 
                    ? new Uri(_httpClient.BaseAddress, url).ToString() 
                    : url;

                var payloadJson = System.Text.Json.JsonSerializer.Serialize(dto, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogInformation("[AuthClient] Register Request: {Payload}", payloadJson);

                var response = await _httpClient.PostAsJsonAsync(url, dto);
                
                _logger.LogInformation("[AuthClient] Register Status: {StatusCode}", response.StatusCode);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("[AuthClient] Register Response: {ResponseBody}", responseContent);
                
                if (!response.IsSuccessStatusCode) 
                {
                    // Try to extract error message from response
                    string errorMessage = "Failed to create account.";
                    
                    if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        // Try to parse as JSON error object
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(responseContent);
                            if (doc.RootElement.TryGetProperty("message", out var message))
                                errorMessage = message.GetString() ?? errorMessage;
                            else if (doc.RootElement.TryGetProperty("error", out var error))
                                errorMessage = error.GetString() ?? errorMessage;
                            else if (doc.RootElement.TryGetProperty("title", out var title))
                                errorMessage = title.GetString() ?? errorMessage;
                            else if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.String)
                                errorMessage = responseContent;
                        }
                        catch
                        {
                            // If not JSON, use the raw content (limit length)
                            errorMessage = responseContent.Length > 200 
                                ? responseContent.Substring(0, 200) + "..." 
                                : responseContent;
                        }
                    }
                    else
                    {
                        errorMessage = $"Failed to create account. Server returned: {response.StatusCode}";
                    }
                    
                    return (null, errorMessage);
                }
                
                try
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<UserLoginResponseDto>(responseContent, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    return (result, null);
                }
                catch (System.Text.Json.JsonException jsonEx)
                {
                    _logger.LogWarning("[AuthClient] Deserialization failed: {Message}", jsonEx.Message);
                    return (null, "Account may have been created, but could not read response.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AuthClient] Exception during registration");
                return (null, $"An error occurred: {ex.Message}");
            }
        }

        public Task<bool> LogoutAsync() =>
            PostAsync<bool>(Endpoints.Auth.Logout, new { });

        public Task<bool> DeleteAccountAsync(int id) =>
            DeleteAsync(Endpoints.Auth.DeleteAccount.Replace("{id}", id.ToString()));
    }
}