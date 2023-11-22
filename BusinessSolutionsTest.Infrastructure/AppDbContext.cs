using BusinessSolutionsTest.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutionsTest.Infrastructure;

public class AppDbContext :  DbContext
{ 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Order?> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Provider>().HasData(
            new Provider
            {
                Id = 1,
                Name = "Провайдер 1"
            },
            new Provider
            {
                Id = 2,
                Name = "Провайдер 2"
            },
            new Provider
            {
                Id = 3,
                Name = "Провайдер 3"
            }
        );
        modelBuilder.Entity<Order>()
            .HasIndex(o => new { o.Number, o.ProviderId })
            .IsUnique();
    }
}