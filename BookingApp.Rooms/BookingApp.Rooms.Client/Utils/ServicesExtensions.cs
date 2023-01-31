using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace BookingApp.Rooms.Client.Utils
{
    public static class ServicesExtensions
    {
        public static IServiceCollection RegisterBookingsClient(this IServiceCollection services, string url)
        {
            services
                .AddRefitClient<IBookingsClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(url));

            return services;
        }
    }
}
