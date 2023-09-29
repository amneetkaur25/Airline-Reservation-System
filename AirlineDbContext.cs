using Microsoft.EntityFrameworkCore;
using Airline_Resevation_System.Models.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Airline_Resevation_System.Models
{
    public class AirlineDbContext:IdentityDbContext
    {
        public AirlineDbContext(DbContextOptions<AirlineDbContext> options) : base(options)
        {
            
        }

        public DbSet<Flight>  Flights{ get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Airline_Resevation_System.Models.ViewModel.BookingViewModel>? BookingViewModel { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            seedRoles(builder);
        }

        private static void seedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name="Admin" ,ConcurrencyStamp="1",NormalizedName="Admin"},
                new IdentityRole() { Name="User" ,ConcurrencyStamp="2",NormalizedName="User"}
                );
        }
    }
}
