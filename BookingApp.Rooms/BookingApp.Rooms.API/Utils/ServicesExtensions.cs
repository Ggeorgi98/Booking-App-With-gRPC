using Microsoft.OpenApi.Models;

namespace BookingApp.Rooms.API.Utils
{
    public static class ServicesExtensions
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services, string swaggerName)
        {
            services.AddGrpcSwagger();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerName, Version = "v1" });
                c.CustomSchemaIds(type => type.ToString());
                c.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "jwt_auth",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()},
                });
            });

            return services;
        }
    }
}
