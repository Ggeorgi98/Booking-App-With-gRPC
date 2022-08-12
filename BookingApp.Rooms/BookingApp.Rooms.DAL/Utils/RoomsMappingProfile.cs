using AutoMapper;
using BookingApp.Rooms.DAL.Entities;
using BookingApp.Rooms.Domain.Dtos;

namespace BookingApp.Users.DAL.Utils
{
    public class RoomsMappingProfile : Profile
    {
        public RoomsMappingProfile()
        {
            CreateMap<Room, RoomDto>();            
            CreateMap<RoomDto, Room>();

            CreateMap<RoomSpecDto, Room>()
                .ForMember(opt => opt.Id, src => src.Ignore());

            CreateMap<Booking, BookRoomsDto>()
                .ForMember(opt => opt.Rooms, src => src.MapFrom(x => x.BookedRooms.Select(t => t.Room)));
            CreateMap<BookedRoom, RoomDto>()
                .ForMember(opt => opt.Id, src => src.MapFrom(x => x.RoomId));
        }
    }
}
