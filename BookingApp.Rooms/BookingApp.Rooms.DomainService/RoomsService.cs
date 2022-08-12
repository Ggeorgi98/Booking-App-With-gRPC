using BookingApp.Rooms.Domain.Dtos;
using BookingApp.Rooms.Domain.Repositories;
using BookingApp.Rooms.Domain.Services;

namespace BookingApp.Rooms.DomainService
{
    public class RoomsService : BaseCrudService<RoomDto>, IRoomsService
    {
        private readonly IRoomsRepository _roomsRepository;

        public RoomsService(IRoomsRepository roomsRepository)
            : base(roomsRepository)
        {
            _roomsRepository = roomsRepository;
        }
    }
}
