using AutoMapper;
using BookingApp.Users.Client;
using BookingApp.Users.Domain.Dtos;

namespace BookingApp.Users.Service.Utils
{
    public class UsersMappingProfile : Profile
    {
        public UsersMappingProfile()
        {
            CreateMap<UserDto, UserResponse>()
                .ForMember(x => x.Id, src => src.MapFrom(opt => opt.Id.ToString()));
        }
    }
}
