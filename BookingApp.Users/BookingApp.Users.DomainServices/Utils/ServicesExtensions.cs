using BookingApp.Users.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Users.DomainServices.Utils
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();

            return services;
        }
    }
}
