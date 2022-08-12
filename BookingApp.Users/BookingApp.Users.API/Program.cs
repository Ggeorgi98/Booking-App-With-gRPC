using AutoMapper;
using BookingApp.Users.API.Utils;
using BookingApp.Users.DAL.Context;
using BookingApp.Users.DAL.Utils;
using BookingApp.Users.DAL.Utils.Extensions;
using BookingApp.Users.DomainServices.Utils;
using BookingApp.Users.Service.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddDbContextFactory<UsersDBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.RegisterSwagger("Booking App - Users API");

builder.Services.Configure<AppSettings>(builder.Configuration);

var appSettings = builder.Configuration.Get<AppSettings>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.RegisterAuth(appSettings);
builder.Services.AddAuthorization();
builder.Services.AddDALUtils(builder.Configuration["CryptographySettings:EncryptionKey"], 
    builder.Configuration["CryptographySettings:EncryptionIV"]);
builder.Services.AddRepositories();
builder.Services.AddServices();

var configuration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new UserMappingProfile(new AESCryptography(appSettings.CryptographySettings.EncryptionKey, 
        appSettings.CryptographySettings.EncryptionIV)));
    cfg.AddProfile(new UsersMappingProfile());
});

var mapper = configuration.CreateMapper();

builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<BookingApp.Users.Service.Services.UsersService>();
app.MapGrpcService<BookingApp.Users.Service.Services.FilesService>();
app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
