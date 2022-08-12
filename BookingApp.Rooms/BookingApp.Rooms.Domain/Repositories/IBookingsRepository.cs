using BookingApp.Rooms.Domain.Dtos;

namespace BookingApp.Rooms.Domain.Repositories
{
    public interface IBookingsRepository : IBaseCrudRepository<BookRoomsDto>
    {
        Task<BookRoomsDto> BookRoomsAsync(BookRoomsSpecDto dto);
    }
}
