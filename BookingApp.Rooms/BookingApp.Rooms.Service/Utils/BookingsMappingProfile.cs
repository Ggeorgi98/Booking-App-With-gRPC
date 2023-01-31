using AutoMapper;
using BookingApp.Rooms.Domain.Dtos;
using BookingApp.Rooms.Grpc.Service;
using Google.Protobuf.WellKnownTypes;

namespace BookingApp.Rooms.Service.Utils
{
    public class BookingsMappingProfile : Profile
    {
        public BookingsMappingProfile()
        {
            CreateMap<BookRoomsDto, BookingResponse>()
                .ForMember(x => x.Id, src => src.MapFrom(opt => opt.Id.ToString()))
                .ForMember(x => x.FromDate, src => src.MapFrom(opt => opt.FromDate.ToTimestamp()))
                .ForMember(x => x.ToDate, src => src.MapFrom(opt => opt.ToDate.ToTimestamp()))
                .ForMember(x => x.Rooms, src => src.MapFrom(opt => opt.Rooms));

            CreateMap<RoomDto, RoomResponse>()
                .ForMember(x => x.Id, src => src.MapFrom(opt => opt.Id.ToString()));
        }
    }
}
