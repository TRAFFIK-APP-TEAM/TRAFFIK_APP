using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json.Serialization;
using TRAFFIK_APP.Configuration;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class VehicleTypeClient : ApiClient
    {
        public VehicleTypeClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<VehicleTypeDto>?> GetAllAsync() =>
            GetAsync<List<VehicleTypeDto>>(Endpoints.VehicleType.GetAll);

        public Task<VehicleTypeDto?> GetByIdAsync(int id) =>
            GetAsync<VehicleTypeDto>(Endpoints.VehicleType.GetById.Replace("{id}", id.ToString()));

        public Task<VehicleTypeDto?> CreateAsync(VehicleTypeDto vehicleType) =>
            PostAsync<VehicleTypeDto>(Endpoints.VehicleType.Create, vehicleType);

        public Task<bool> UpdateAsync(int id, VehicleTypeDto vehicleType) =>
            PutAsync(Endpoints.VehicleType.Update.Replace("{id}", id.ToString()), vehicleType);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.VehicleType.Delete.Replace("{id}", id.ToString()));
    }

    // DTO for VehicleType
    public class VehicleTypeDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }
}

