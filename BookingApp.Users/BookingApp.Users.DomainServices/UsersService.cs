using AutoMapper;
using BookingApp.Rooms.Client;
using BookingApp.Users.Domain.Dtos;
using BookingApp.Users.Domain.Repositories;
using BookingApp.Users.Domain.Services;
using BookingApp.Users.DomainServices.Utils;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingApp.Users.DomainServices
{
    public class UsersService : BaseCrudService<UserDto>, IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IBookingsClient _bookingsClient;
        private readonly IMapper _mapper;
        private readonly JwtSettings _options;

        public UsersService(IUsersRepository userRepository, IOptions<JwtSettings> options, IBookingsClient bookingsClient,
            IMapper mapper)
            : base(userRepository)
        {
            _usersRepository = userRepository;
            _options = options.Value;
            _bookingsClient = bookingsClient;
            _mapper = mapper;
        }

        public async Task<List<string>> GetExistingUserEmailsAsync(IEnumerable<string> userEmails, Guid userId)
        {
            return await _usersRepository
                .GetExistingUserEmailsAsync(userEmails, userId)
                .ConfigureAwait(false);
        }

        public async Task<UserDto> CreateUserAsync(UserSpecDto item)
        {
            item.IsActive = true;

            return await _usersRepository.CreateUserAsync(item);
        }

        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ValidationDictionary.AddModelError("Credentials required", "The email and the password are required");
                return null!;
            }

            var passwordSalt = await _usersRepository.GetUserAuthenticateInfoAsync(email);
            if (string.IsNullOrEmpty(passwordSalt))
            {
                ValidationDictionary.AddModelError("Invalid credentials", "The email or the password is invalid");
                return null!;
            }

            try
            {
                var userTokenData = await _usersRepository.AuthenticateUserAsync(email, password, passwordSalt);
                if (userTokenData == null)
                {
                    ValidationDictionary.AddModelError("Invalid credentials", "The email or the password is invalid");
                    return null!;
                }

                return GenerateToken(userTokenData);
            }
            catch (Exception)
            {
                ValidationDictionary.AddModelError("Invalid credentials", "The email or the password is invalid");
                return null!;
            }

        }

        public async Task<UserFullDataDto> UserFullProfileData(Guid userId)
        {
            var user = await GetByIdAsync(userId);

            var userFullData = _mapper.Map<UserFullDataDto>(user);
            userFullData.BookingData = await _bookingsClient.GetLastUserBookingAsync(userId);

            return userFullData;
        }

        private string GenerateToken(UserTokenDto user)
        {
            var claims = PopulateTokenClaims(user);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
               issuer: _options.Issuer,
               audience: _options.Issuer,
               claims: claims,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        private static List<Claim> PopulateTokenClaims(UserTokenDto user)
        {
            return new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
        }
    }
}
