using AutoMapper;
using BookingApp.Users.DAL.Context;
using BookingApp.Users.DAL.Entities;
using BookingApp.Users.Domain;
using BookingApp.Users.Domain.Dtos;
using BookingApp.Users.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Users.DAL.Repositories
{
    public class UsersRepository : BaseCrudRepository<User, UserDto>, IUsersRepository
    {
        private readonly IAESCryptography _aESCryptography;
        private readonly IPasswordHashingHelper _passwordHashingHelper;

        public UsersRepository(IDbContextFactory<UsersDBContext> dbContext, IMapper mapper,
            IAESCryptography aESCryptography, IPasswordHashingHelper passwordHashingHelper)
            : base(dbContext, mapper)
        {
            _aESCryptography = aESCryptography;
            _passwordHashingHelper = passwordHashingHelper;
        }

        public async Task<UserTokenDto> AuthenticateUserAsync(string email, string password, string passwordSalt)
        {
            await using var context = _dbContext.CreateDbContext();

            var hashedPass = _passwordHashingHelper.HashPassword(password, passwordSalt);

            var userInfo = await context.Users
                .AsNoTracking()
                .Where(item => _aESCryptography.Encrypt(email) == item.Email && item.Password == hashedPass)
                .Select(x => new
                {
                    x.Id,
                    Email = _aESCryptography.Decrypt(x.Email),
                    x.Role
                })
                .FirstAsync()
                .ConfigureAwait(false);

            return new UserTokenDto
            {
                UserId = userInfo.Id,
                Email = userInfo.Email,
                Role = userInfo.Role
            };
        }

        public async Task<List<string>> GetExistingUserEmailsAsync(IEnumerable<string> userEmails, Guid userId)
        {
            if (userEmails == null)
            {
                throw new ArgumentNullException(nameof(userEmails));
            }

            var encryptedEmails = new List<string>();
            foreach (var email in userEmails)
            {
                encryptedEmails.Add(_aESCryptography.Encrypt(email));
            }

            await using var context = _dbContext.CreateDbContext();
            var existingEmails = await context.Users
                .Where(item => encryptedEmails.Contains(item.Email) && item.Id != userId)
                .Select(x => _aESCryptography.Decrypt(x.Email))
                .ToListAsync()
                .ConfigureAwait(false);

            return existingEmails;
        }

        public async Task<string> GetUserAuthenticateInfoAsync(string email)
        {
            await using var context = _dbContext.CreateDbContext();
            var passwordSalt = await context.Users
                .AsNoTracking()
                .Where(item => _aESCryptography.Encrypt(email) == item.Email)
                .Select(x => x.PasswordSalt)
                .FirstAsync()
                .ConfigureAwait(false);

            return passwordSalt;
        }

        public async Task<UserDto> CreateUserAsync(UserSpecDto userSpec)
        {
            await using var context = _dbContext.CreateDbContext();
            var items = context.Set<User>();

            var entity = Mapper.Map<UserSpecDto, User>(userSpec);
            entity.Id = Guid.NewGuid();
            entity.PasswordSalt = _passwordHashingHelper.GenerateSalt();
            entity.Password = _passwordHashingHelper.HashPassword(userSpec.Password, entity.PasswordSalt);

            await items.AddAsync(entity).ConfigureAwait(false);

            await context.SaveChangesAsync().ConfigureAwait(false);

            return Mapper.Map<UserDto>(entity);
        }
    }
}
