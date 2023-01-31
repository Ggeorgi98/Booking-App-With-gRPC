using AutoMapper;
using BookingApp.Rooms.API.Utils;
using BookingApp.Rooms.DAL.Context;
using BookingApp.Rooms.Service.Utils;
using BookingApp.Users.DAL.Utils;
using BookingApp.Users.DAL.Utils.Extensions;
using BookingApp.Users.DomainServices.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<RoomsContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseLazyLoadingProxies();
});

builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddControllers();
builder.Services.RegisterSwagger("Booking App - Rooms API");
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddCustomGrpc(builder.Configuration["UsersClient"]);

var configuration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(typeof(RoomsMappingProfile));
    cfg.AddProfile(new BookingsMappingProfile());
});

var mapper = configuration.CreateMapper();

builder.Services.AddSingleton(mapper);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = true,
                       ValidateIssuer = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidAudience = "",
                       ValidIssuer = "",
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test"))
                   };
               });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<BookingApp.Rooms.Service.BookingsService>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
