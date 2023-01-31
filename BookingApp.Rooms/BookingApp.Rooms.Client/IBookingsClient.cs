using BookingApp.Rooms.Client.ResponseModels;
using Refit;

namespace BookingApp.Rooms.Client
{
    public interface IBookingsClient
    {
        [Get("/bookings/last-user-booking/{id}")]
        Task<BookingResponse> GetLastUserBookingAsync(Guid id);
    }
}