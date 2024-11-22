using Microsoft.EntityFrameworkCore;
using RealEstateAgency.Domain;

namespace RealEstateAgency.Data;

public class RealEstateAgencyContext : DbContext
{
    public RealEstateAgencyContext(DbContextOptions<RealEstateAgencyContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<RealEstate> RealEstates { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Client)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.RealEstate)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
