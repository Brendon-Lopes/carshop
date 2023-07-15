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
        modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<User>().Property(u => u.UpdatedAt).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        modelBuilder.Entity<Brand>().Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<Brand>().Property(b => b.UpdatedAt).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<Brand>().HasIndex(b => b.Name).IsUnique();
        modelBuilder.Entity<Brand>().HasKey(b => b.Id);

        modelBuilder.Entity<Car>().Property(c => c.CreatedAt).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<Car>().Property(c => c.UpdatedAt).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<Car>().Property(c => c.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Car>().HasKey(c => c.Id);
    }
}