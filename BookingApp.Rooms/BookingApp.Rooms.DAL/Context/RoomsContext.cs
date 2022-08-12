using BookingApp.Rooms.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Rooms.DAL.Context
{
    public class RoomsContext : DbContext
    {
        #region DBSets

        public DbSet<Room> Rooms { get; set; }
        public DbSet<BookedRoom> BookedRooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        #endregion

        public RoomsContext(DbContextOptions<RoomsContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>()
                .HasIndex(b => b.Id);

            modelBuilder.Entity<Booking>()
               .HasIndex(b => b.Id);

            modelBuilder.Entity<Booking>()
                .HasMany(b => b.BookedRooms)
                .WithOne(c => c.Booking)
                .HasForeignKey(x => x.BoookingId);

            modelBuilder.Entity<Room>()
                .HasMany(b => b.BookedRooms)
                .WithOne(c => c.Room)
                .HasForeignKey(x => x.RoomId);
        }
    }

    public class RoomsContextFactory : IDesignTimeDbContextFactory<RoomsContext>
    {
        public RoomsContext CreateDbContext(string[] args)
        {
            var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "BookingApp.Rooms.API";
            var optionsBuilder = new DbContextOptionsBuilder<RoomsContext>();
            var config = new ConfigurationBuilder()
               .SetBasePath(path)
               .AddJsonFile("appsettings.json")
               .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            return new RoomsContext(optionsBuilder.Options);
        }
    }
}