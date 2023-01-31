using AutoMapper;
using BookingApp.Users.DAL.Entities;
using BookingApp.Users.Domain;
using BookingApp.Users.Domain.Dtos;
using System.Globalization;

namespace BookingApp.Users.DAL.Utils
{
    public class UserMappingProfile : Profile
    {
        private readonly IAESCryptography _aESCryptography;

        public UserMappingProfile(IAESCryptography aESCryptography)
        {
            _aESCryptography = aESCryptography;

            CreateMap<User, UserDto>()
                .ForMember(opt => opt.Email,
                    src => src.MapFrom(x => _aESCryptography.Decrypt(x.Email)))
                .ForMember(opt => opt.FirstName,
                    src => src.MapFrom(x => _aESCryptography.Decrypt(x.FirstName)))
                .ForMember(opt => opt.LastName,
                    src => src.MapFrom(x => _aESCryptography.Decrypt(x.LastName)))
                .ForMember(opt => opt.PhoneNumber,
                    src => src.MapFrom(x => _aESCryptography.Decrypt(x.PhoneNumber)));                       

            CreateMap<UserDto, User>()
                .ForMember(opt => opt.Email,
                    src => src.MapFrom(x => _aESCryptography.Encrypt(x.Email.ToLower(CultureInfo.InvariantCulture))))
                .ForMember(opt => opt.FirstName,
                    src => src.MapFrom(x => _aESCryptography.Encrypt(x.FirstName)))
                .ForMember(opt => opt.LastName,
                    src => src.MapFrom(x => _aESCryptography.Encrypt(x.LastName)))
                .ForMember(opt => opt.PhoneNumber,
                    src => src.MapFrom(x => _aESCryptography.Encrypt(x.PhoneNumber)));

            CreateMap<UserSpecDto, User>()
                .ForMember(opt => opt.Email,
                    src => src.MapFrom(x => _aESCryptography.Encrypt(x.Email.ToLower(CultureInfo.InvariantCulture))))
                .ForMember(opt => opt.FirstName,
                    src => src.MapFrom(x => _aESCryptography.Encrypt(x.FirstName)))
                .ForMember(opt => opt.LastName,
                    src => src.MapFrom(x => _aESCryptography.Encrypt(x.LastName)))
                .ForMember(opt => opt.PhoneNumber,
                    src => src.MapFrom(x => _aESCryptography.Encrypt(x.PhoneNumber)))
                .ForMember(opt => opt.Id, src => src.Ignore());

            CreateMap<UserDto, UserFullDataDto>();
        }
    }
}
