using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Dtos.Vehicle;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class VehicleClient : ApiClient
    {
        public VehicleClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public async Task<List<VehicleDto>> GetByUserAsync(int userId)
        {
            var endpoint = Endpoints.Vehicle.GetByUser.Replace("{userId}", userId.ToString());
            return await GetAsync<List<VehicleDto>>(endpoint);
        }

        public async Task<VehicleDto?> GetByIdAsync(int id)
        {
            var endpoint = Endpoints.Vehicle.GetById.Replace("{id}", id.ToString());
            return await GetAsync<VehicleDto>(endpoint);
        }

        public async Task<VehicleDto?> CreateAsync(VehicleDto dto)
        {
            var endpoint = Endpoints.Vehicle.Create;
            return await PostAsync<VehicleDto>(endpoint, dto);
        }

        public async Task<bool> UpdateAsync(int id, VehicleDto dto)
        {
            var endpoint = Endpoints.Vehicle.UpdateById.Replace("{id}", id.ToString());
            return await PutAsync(endpoint, dto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var endpoint = Endpoints.Vehicle.DeleteById.Replace("{id}", id.ToString());
            return await DeleteAsync(endpoint);
        }

        public async Task<List<string>> GetAllVehicleTypesAsync()
        {
            var response = await _httpClient.GetAsync("api/Vehicle/Types");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<string>>(json);
            }
            return new List<string>();
        }
    }
}