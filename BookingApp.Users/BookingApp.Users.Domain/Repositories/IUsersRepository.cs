using BookingApp.Users.Domain.Dtos;

namespace BookingApp.Users.Domain.Repositories
{
    public interface IUsersRepository : IBaseCrudRepository<UserDto>
    {
        Task<UserTokenDto> AuthenticateUserAsync(string email, string password, string passwordSalt);

        Task<List<string>> GetExistingUserEmailsAsync(IEnumerable<string> userEmails, Guid userId);

        Task<string> GetUserAuthenticateInfoAsync(string email);

        Task<UserDto> CreateUserAsync(UserSpecDto userSpec);
    }
}
