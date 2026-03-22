using Microsoft.EntityFrameworkCore;

namespace Library.EfCore;

//Task 1.1
public class ShopContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
            "Server=localhost;Database=ShopDB;User Id=sa;Password=yourStrong(!)Password;Encrypt=False"
        );
        //Task 1.2
        // options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        // options.EnableSensitiveDataLogging();
    }
}