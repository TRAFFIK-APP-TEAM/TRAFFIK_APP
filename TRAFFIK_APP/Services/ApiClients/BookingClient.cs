using TRAFFIK_APP.Configuration;
using TRAFFIK_APP.Models.Entities.Booking;
using TRAFFIK_APP.Models.Dtos.Booking;
using Microsoft.Extensions.Logging;

namespace TRAFFIK_APP.Services.ApiClients
{
    public class BookingClient : ApiClient
    {
        public BookingClient(HttpClient httpClient, ILogger<ApiClient> logger) : base(httpClient, logger) { }

        public Task<List<BookingDto>?> GetAllAsync() =>
            GetAsync<List<BookingDto>>(Endpoints.Booking.GetAll);

        public Task<List<BookingDto>?> GetStaffBookingsAsync() =>
            GetAsync<List<BookingDto>>(Endpoints.Booking.GetStaffBookings);

        public Task<Booking?> GetByIdAsync(int id) =>
            GetAsync<Booking>(Endpoints.Booking.GetById.Replace("{id}", id.ToString()));

        public Task<Booking?> CreateAsync(Booking booking)
        {
            // Wrap the booking object to match API's expected JSON shape (BookingRequest wrapper)
            var payload = new BookingRequest { Booking = booking };
            return PostAsync<Booking>(Endpoints.Booking.Create, payload);
        }
        
        public Task<Booking?> CreateAsync(BookingCreateDto bookingDto)
        {
            // Create Booking from DTO
            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                ServiceCatalogId = bookingDto.ServiceCatalogId,
                VehicleLicensePlate = bookingDto.VehicleLicensePlate,
                BookingDate = bookingDto.BookingDate,
                BookingTime = bookingDto.BookingTime,
                Status = bookingDto.Status
            };
            
            // Wrap in BookingRequest
            var payload = new BookingRequest { Booking = booking };
            return PostAsync<Booking>(Endpoints.Booking.Create, payload);
        }

        public Task<bool> UpdateAsync(int id, Booking booking) =>
            PutAsync(Endpoints.Booking.UpdateById.Replace("{id}", id.ToString()), booking);

        public Task<bool> DeleteAsync(int id) =>
            base.DeleteAsync(Endpoints.Booking.DeleteById.Replace("{id}", id.ToString()));

        public async Task<List<BookingDto>> GetByUserAsync(int userId)
        {
            var endpoint = Endpoints.Booking.GetByUser.Replace("{userId}", userId.ToString());
            return await GetAsync<List<BookingDto>>(endpoint);
        }

        public async Task<List<TimeOnly>?> GetAvailableSlotsAsync(int serviceCatalogId, string date)
        {
            var endpoint = $"{Endpoints.Booking.AvailableSlots}?serviceCatalogId={serviceCatalogId}&date={date}";
            return await GetAsync<List<TimeOnly>>(endpoint);
        }

        public async Task<Booking?> ConfirmBookingAsync(BookingCreateDto bookingDto)
        {
            // Create Booking from DTO
            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                ServiceCatalogId = bookingDto.ServiceCatalogId,
                VehicleLicensePlate = bookingDto.VehicleLicensePlate,
                BookingDate = bookingDto.BookingDate,
                BookingTime = bookingDto.BookingTime,
                Status = "Pending" // API sets this
            };
            
            // Wrap in BookingRequest
            var payload = new BookingRequest { Booking = booking };
            return await PostAsync<Booking>(Endpoints.Booking.Confirm, payload);
        }
        
        public class BookingRequest
        {
            public Booking Booking { get; set; } = null!;
        }
    }
}