using Microsoft.Extensions.Logging;
using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Dtos.Car;
using TRAFFIK_APP.Models.Entities.CarModel;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class CarModelClient : ApiClient
    {
        public CarModelClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<CarModel?> CreateAsync(CarModel carModel) =>
            PostAsync<CarModel>(Endpoints.CarModel.Create, carModel);

        public Task<CarModel?> GetByIdAsync(int id) =>
            GetAsync<CarModel>(Endpoints.CarModel.GetById.Replace("{id}", id.ToString()));

        public Task<List<CarModel>?> GetByUserAsync(int userId) =>
            GetAsync<List<CarModel>>(Endpoints.CarModel.GetByUser.Replace("{userId}", userId.ToString()));

        public async Task<int> CreateOrGetAsync(int userId, string vehicleType, string make, string model, string plateNumber, int year)
        {
            var dto = new CarModelCreateDto
            {
                UserId = userId,
                VehicleType = vehicleType,
                Make = make,
                Model = model,
                PlateNumber = plateNumber,
                Year = year
            };

            var carModel = await PostAsync<CarModel>(Endpoints.CarModel.CreateOrGet, dto);
            if (carModel == null)
                throw new Exception("Failed to create or get CarModel");

            return carModel.Id;
        }
    }
}
