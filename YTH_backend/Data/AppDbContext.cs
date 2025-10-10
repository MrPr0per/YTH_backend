using Microsoft.EntityFrameworkCore;

namespace YTH_backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Example
    // public DbSet<Entity> Entities { get; set; } = null!;
    //
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //
    //     modelBuilder.Entity<Entity>(b =>
    //     {
    //         b.HasKey(e => e.Id);
    //         b.Property(e => e.Name).IsRequired().HasMaxLength(200);
    //         // Добавь конфигурации индексов, связей и т.д.
    //     });
    // }
}