using System.Net.Http.Json;
using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class ApiClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger _logger;

        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(Endpoints.BaseUrl);
        }


        protected async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }
                
                var content = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<T>(content, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApiClient] Exception in GetAsync: {ex.Message}");
                return default;
            }
        }

        protected async Task<T?> PostAsync<T>(string url, object payload)
        {
            try
            {
                // Log the request details
                var fullUrl = _httpClient.BaseAddress != null 
                    ? new Uri(_httpClient.BaseAddress, url).ToString() 
                    : url;

                var payloadJson = System.Text.Json.JsonSerializer.Serialize(payload, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogInformation("[API] Request Payload: {Payload}", payloadJson);

                var response = await _httpClient.PostAsJsonAsync(url, payload);
                
                _logger.LogInformation("[API] Status: {StatusCode}", response.StatusCode);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("[API] Response Body: {ResponseBody}", responseContent);
                
                // DEBUG: Show response in popup
                
                if (!response.IsSuccessStatusCode) 
                {
                    _logger.LogWarning("[API] Request failed with status {StatusCode} - returning null", response.StatusCode);
                    return default;
                }
                
                _logger.LogInformation("[API] Attempting to deserialize to {TypeName}", typeof(T).Name);
                
                try
                {
                    var result = System.Text.Json.JsonSerializer.Deserialize<T>(responseContent, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    
                    _logger.LogInformation("[API] Deserialization result: {Result}", result != null ? "SUCCESS" : "NULL");
                    
                    return result;
                }
                catch (System.Text.Json.JsonException jsonEx)
                {
                    _logger.LogWarning("[API] Deserialization failed (vehicle was likely created): {Message}", jsonEx.Message);
                    return default; // Return null on deserialization error
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "[API] HTTP request exception: {Message}", ex.Message);
                _logger.LogError(ex, "[API] Inner exception: {InnerException}", ex.InnerException?.Message);
                await Application.Current.MainPage.DisplayAlert("API Error", $"HTTP Exception: {ex.Message}\nInner: {ex.InnerException?.Message}", "OK");
                return default;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "[API] Request timeout");
                await Application.Current.MainPage.DisplayAlert("API Error", "Request timed out", "OK");
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[API] Unexpected exception");
                await Application.Current.MainPage.DisplayAlert("API Error", $"Exception: {ex.Message}", "OK");
                return default;
            }
        }

        protected async Task<bool> PutAsync(string url, object payload)
        {
            var response = await _httpClient.PutAsJsonAsync(url, payload);
            return response.IsSuccessStatusCode;
        }

        protected async Task<bool> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}