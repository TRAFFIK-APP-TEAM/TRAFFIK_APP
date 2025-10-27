using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Dtos.Vehicle;
using System.Text.Json.Serialization;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class VehicleClient : ApiClient
    {
        public VehicleClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public async Task<List<VehicleDto>> GetByUserAsync(int userId)
        {
            try
            {
                var endpoint = Endpoints.Vehicle.GetByUser.Replace("{userId}", userId.ToString());
                var result = await GetAsync<List<VehicleDto>>(endpoint);
                return result ?? new List<VehicleDto>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VehicleClient] Error in GetByUserAsync: {ex.Message}");
                return new List<VehicleDto>();
            }
        }

        public async Task<List<VehicleDto>> GetAllAsync()
        {
            try
            {
                var result = await GetAsync<List<VehicleDto>>(Endpoints.Vehicle.GetAll);
                return result ?? new List<VehicleDto>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VehicleClient] Error in GetAllAsync: {ex.Message}");
                return new List<VehicleDto>();
            }
        }

        public async Task<VehicleDto?> GetByLicensePlateAsync(string licensePlate)
        {
            try
            {
                var endpoint = Endpoints.Vehicle.GetByLicensePlate.Replace("{licensePlate}", licensePlate);
                return await GetAsync<VehicleDto>(endpoint);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VehicleClient] Error in GetByLicensePlateAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<VehicleDto?> CreateAsync(VehicleDto dto)
        {
            var endpoint = Endpoints.Vehicle.Create;
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, dto);
                
                System.Diagnostics.Debug.WriteLine($"[VehicleClient] CreateAsync response status: {response.StatusCode}");
                
                if (response.IsSuccessStatusCode)
                {
                    // Try to deserialize, but if it fails, the vehicle was still created
                    var content = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[VehicleClient] Response content: {content}");
                    
                    try
                    {
                        var result = System.Text.Json.JsonSerializer.Deserialize<VehicleDto>(content, new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        System.Diagnostics.Debug.WriteLine("[VehicleClient] Deserialization successful");
                        return result;
                    }
                    catch (Exception deserializeEx)
                    {
                        // Deserialization failed but request succeeded - return a minimal DTO
                        System.Diagnostics.Debug.WriteLine($"[VehicleClient] Vehicle created but response deserialization failed: {deserializeEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"[VehicleClient] Returned original dto");
                        return dto; // Return the input DTO as a successful result
                    }
                }
                
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[VehicleClient] Request failed with status {response.StatusCode}: {errorContent}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VehicleClient] Error creating vehicle: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[VehicleClient] Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string licensePlate, VehicleDto dto)
        {
            var endpoint = Endpoints.Vehicle.Update.Replace("{licensePlate}", licensePlate);
            return await PutAsync(endpoint, dto);
        }

        public async Task<bool> DeleteAsync(string licensePlate)
        {
            var endpoint = Endpoints.Vehicle.Delete.Replace("{licensePlate}", licensePlate);
            return await DeleteAsync(endpoint);
        }

        public async Task<List<VehicleTypeDto>> GetAllVehicleTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{Endpoints.BaseUrl}/api/VehicleTypes");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    
                    var types = JsonSerializer.Deserialize<List<VehicleTypeDto>>(json, options);
                    
                    // If deserialization fails, try parsing as List<string>
                    if (types == null)
                    {
                        var stringList = JsonSerializer.Deserialize<List<string>>(json, options) ?? new List<string>();
                        return stringList.Select(t => new VehicleTypeDto { Type = t }).ToList();
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"[VehicleClient] Loaded {types.Count} vehicle types");
                    
                    return types;
                }
                return new List<VehicleTypeDto>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[VehicleClient] Error in GetAllVehicleTypesAsync: {ex.Message}");
                return new List<VehicleTypeDto>();
            }
        }
    }
}