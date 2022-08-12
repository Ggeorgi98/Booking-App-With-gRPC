using BookingApp.Rooms.Domain.Dtos;

namespace BookingApp.Rooms.Domain.Repositories
{
    public interface IRoomsRepository : IBaseCrudRepository<RoomDto>
    {
    }
}
