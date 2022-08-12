using BookingApp.Rooms.DAL.Repositories;
using BookingApp.Rooms.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Users.DAL.Utils.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoomsRepository, RoomsRepository>();
            services.AddScoped<IBookingsRepository, BookingsRepository>();

            return services;
        }
    }
}
