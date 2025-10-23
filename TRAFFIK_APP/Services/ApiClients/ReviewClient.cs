using TRAFFIK_APP.Configuration;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class ReviewClient : ApiClient
    {
        public ReviewClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<Review>?> GetAllAsync() =>
            GetAsync<List<Review>>(Endpoints.Review.GetAll);

        public Task<Review?> GetByIdAsync(int id) =>
            GetAsync<Review>(Endpoints.Review.GetById.Replace("{id}", id.ToString()));

        public Task<Review?> CreateAsync(Review review) =>
            PostAsync<Review>(Endpoints.Review.Create, review);

        public Task<bool> UpdateAsync(int id, Review review) =>
            PutAsync(Endpoints.Review.UpdateById.Replace("{id}", id.ToString()), review);

        public Task<bool> DeleteAsync(int id) =>
            DeleteAsync(Endpoints.Review.DeleteById.Replace("{id}", id.ToString()));
    }
}