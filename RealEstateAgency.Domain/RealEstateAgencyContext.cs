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

            entity.HasMany(c => c.Orders)
                  .WithOne(o => o.Client)
                  .HasForeignKey(o => o.ClientId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(c => c.FirstAndLastName).IsRequired();
            entity.Property(c => c.Pasport).IsRequired();
            entity.Property(c => c.Address).IsRequired();
        });

        modelBuilder.Entity<RealEstate>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasMany(r => r.Orders)
                  .WithOne(o => o.RealEstate)
                  .HasForeignKey(o => o.RealEstateId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(r => r.Address).IsRequired();

            entity.Property(r => r.Type)
                  .HasConversion<string>();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.HasOne(o => o.Client)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.ClientId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.RealEstate)
                  .WithMany(r => r.Orders)
                  .HasForeignKey(o => o.RealEstateId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(o => o.Type)
                  .HasConversion<string>();

            entity.Property(o => o.Time).IsRequired();
            entity.Property(o => o.Price).IsRequired();
        });
    }
}
