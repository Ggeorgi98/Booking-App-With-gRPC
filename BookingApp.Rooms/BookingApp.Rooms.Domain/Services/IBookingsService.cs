using BookingApp.Rooms.Domain.Dtos;
using System.Linq.Expressions;

namespace BookingApp.Rooms.Domain.Services
{
    public interface IBookingsService : IBaseCrudService<BookRoomsDto>
    {
        Task<BookRoomsDto> BookRoomsAsync(BookRoomsSpecDto dto);
        Task<PagedResults<BookRoomsDto>> GetUserBookingsAsync(Guid userId);
    }
}
