using Microsoft.EntityFrameworkCore;

namespace RealEstateAgency.Domain;

public class RealEstateAgencyContext(DbContextOptions<RealEstateAgencyContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<RealEstate> RealEstates { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.FirstAndLastName).IsRequired();
            entity.Property(c => c.Pasport).IsRequired();
            entity.Property(c => c.Address).IsRequired();
        });

        modelBuilder.Entity<RealEstate>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Address).IsRequired();

            entity.Property(r => r.Type)
                  .HasConversion<string>();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Type)
                  .HasConversion<string>();

            entity.Property(o => o.Time).IsRequired();
            entity.Property(o => o.Price).IsRequired();
        });
    }
}
