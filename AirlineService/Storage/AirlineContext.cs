using AirlineService.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineService.Storage
{
    public class AirlineContext : DbContext
    {
        public AirlineContext(DbContextOptions<AirlineContext> options)
            : base(options)
        { }

        public DbSet<FlightRouteRaw> FlightRoute { get; set; }
        public DbSet<FlightRaw> Flight { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlightRouteRaw>().ToTable("FlightRoutes");
            modelBuilder.Entity<FlightRaw>().ToTable("Flights");
        }
    }
}
