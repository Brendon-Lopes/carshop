using Carshop.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Carshop.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        modelBuilder.Entity<Brand>().HasIndex(b => b.Name).IsUnique();
        modelBuilder.Entity<Brand>().HasKey(b => b.Id);

        modelBuilder.Entity<Car>().Property(c => c.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Car>().HasKey(c => c.Id);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        var currentTime = DateTime.UtcNow;

        foreach (var entityEntry in entities)
        {
            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = currentTime;
            }

            ((BaseEntity)entityEntry.Entity).UpdatedAt = currentTime;
        }
    }
}