using Microsoft.EntityFrameworkCore;
using WebProj2.Models;

namespace WebProj2;

public class ApplicationDbContext : DbContext
{
    
    public DbSet<Product> Products { get; set; }   
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}