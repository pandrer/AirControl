using HangarService.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace HangarService.Storage
{
    public class HangarContext : DbContext
    {
        public HangarContext(DbContextOptions<HangarContext> options)
            : base(options)
        { }

        public DbSet<AircraftRaw> Aircrafts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AircraftRaw>().ToTable("Aircrafts");
        }

    }
}
