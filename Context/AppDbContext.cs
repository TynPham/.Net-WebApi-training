using Microsoft.EntityFrameworkCore;
using WebApi.Model;

namespace WebApi.Context;

public class AppDbContext : DbContext
{
    public  AppDbContext(DbContextOptions dnContextOptions) : base(dnContextOptions)
    {
        
    }
    
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
}