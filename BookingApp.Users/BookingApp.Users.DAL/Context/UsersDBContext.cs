using BookingApp.Users.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BookingApp.Users.DAL.Context
{
    public class UsersDBContext : DbContext
    {
        #region DBSets
        public DbSet<User> Users { get; set; }
        #endregion

        public UsersDBContext(DbContextOptions<UsersDBContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(b => b.Id);
        }
    }

    public class UsersDBContextFactory : IDesignTimeDbContextFactory<UsersDBContext>
    {
        public UsersDBContext CreateDbContext(string[] args)
        {
            var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "BookingApp.Users.API";
            var optionsBuilder = new DbContextOptionsBuilder<UsersDBContext>();
            var config = new ConfigurationBuilder()
               .SetBasePath(path)
               .AddJsonFile("appsettings.json")
               .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));

            return new UsersDBContext(optionsBuilder.Options);
        }
    }
}
