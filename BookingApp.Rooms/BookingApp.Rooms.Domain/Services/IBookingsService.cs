using BookingApp.Rooms.Domain.Dtos;

namespace BookingApp.Rooms.Domain.Services
{
    public interface IBookingsService : IBaseCrudService<BookRoomsDto>
    {
        Task<BookRoomsDto> BookRoomsAsync(BookRoomsSpecDto dto);
    }
}
