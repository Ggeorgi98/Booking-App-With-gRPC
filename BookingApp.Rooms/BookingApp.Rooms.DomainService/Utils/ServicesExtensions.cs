using BookingApp.Rooms.Domain.Services;
using BookingApp.Rooms.Domain.Utils;
using BookingApp.Rooms.DomainService;
using BookingApp.Rooms.DomainService.Utils;
using BookingApp.Users.Client;
using Microsoft.Extensions.DependencyInjection;


namespace BookingApp.Users.DomainServices.Utils
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomsService, RoomsService>();
            services.AddScoped<IBookingsService, BookingsService>();
            services.AddScoped<IBookingFilesService, BookingFilesService>();
            services.AddScoped<ITokenProvider, TokenProvider>();

            return services;
        }

        public static IServiceCollection AddCustomGrpc(this IServiceCollection services, string usersClientUrl)
        {
            services.AddGrpcClient<UsersService.UsersServiceClient>(o =>
            {
                o.Address = new Uri(usersClientUrl);
            })
            .AddCallCredentials(async (context, metadata, serviceProvider) =>
            {
                var provider = serviceProvider.GetRequiredService<ITokenProvider>();
                var token = await provider.GetTokenAsync();
                metadata.Add("Authorization", $"{token}");
            });

            services.AddGrpcClient<FilesService.FilesServiceClient>(o =>
            {
                o.Address = new Uri(usersClientUrl);
            });

            return services;
        }
    }
}
