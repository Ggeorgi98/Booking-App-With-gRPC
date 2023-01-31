using BookingApp.Users.Domain.Dtos;

namespace BookingApp.Users.Domain.Services
{
    public interface IUsersService : IBaseCrudService<UserDto>
    {
        Task<List<string>> GetExistingUserEmailsAsync(IEnumerable<string> userEmails, Guid userId);

        Task<UserDto> CreateUserAsync(UserSpecDto user);

        Task<string> AuthenticateUserAsync(string email, string password);

        Task<UserFullDataDto> UserFullProfileData(Guid userId);
    }
}
