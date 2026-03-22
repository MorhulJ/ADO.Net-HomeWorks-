using Microsoft.EntityFrameworkCore;

namespace Library.EfCore;

//Task 1.1
public class GameContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
            "Server=localhost;Database=SteamDb;User Id=sa;Password=yourStrong(!)Password;Encrypt=False"
        );
        // options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        // options.EnableSensitiveDataLogging();
    }
}