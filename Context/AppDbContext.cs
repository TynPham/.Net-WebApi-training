using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Context;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public  AppDbContext(DbContextOptions dnContextOptions) : base(dnContextOptions)
    {
        
    }
    
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<Portfolio> Portfolios { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Portfolio>().HasKey(p => new {p.AppUserId, p.StockId});
        
        modelBuilder.Entity<Portfolio>()
            .HasOne(p => p.AppUser)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.AppUserId);
        
        modelBuilder.Entity<Portfolio>()
            .HasOne(p => p.Stock)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(p => p.StockId);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            }
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}