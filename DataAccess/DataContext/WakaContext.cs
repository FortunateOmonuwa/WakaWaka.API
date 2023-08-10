using Microsoft.EntityFrameworkCore;
using WakaWaka.API.Domain.Models.Hotel;
using WakaWaka.API.Models.Hotel;
using WakaWaka.API.Models.Restaurant;
using WakaWaka.API.Models.Resturant;

namespace WakaWaka.API.DataAccessLayer.DataContext
{
    public class WakaContext :DbContext 
    {
        public WakaContext(DbContextOptions<WakaContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           // modelBuilder.Entity<Hotel>()
                
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Restaurant> Restaurants { get; set;}
        public DbSet<HotelReview> HotelReviews { get; set;}
        public DbSet<RestaurantReview> RestaurantReviews { get;set; }
    }
}
