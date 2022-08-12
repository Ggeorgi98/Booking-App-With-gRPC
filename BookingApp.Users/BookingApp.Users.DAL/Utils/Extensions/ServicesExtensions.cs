using BookingApp.Users.DAL.Repositories;
using BookingApp.Users.Domain;
using BookingApp.Users.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Users.DAL.Utils.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }

        public static IServiceCollection AddDALUtils(this IServiceCollection services, string encryptionKey, string encryptionIV)
        {
            services.AddScoped<IAESCryptography>(x => new AESCryptography(encryptionKey, encryptionIV));
            services.AddScoped<IPasswordHashingHelper, PasswordHashingHelper>();

            return services;
        }
    }
}
