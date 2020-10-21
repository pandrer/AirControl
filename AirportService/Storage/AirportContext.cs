using AirportService.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace AirportService.Storage
{
    public class AirportContext : DbContext
    {
        public AirportContext(DbContextOptions<AirportContext> options)
            : base(options)
        { }

        public DbSet<AirportRaw> Airports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AirportRaw>().ToTable("Airports");
        }
    }
}
