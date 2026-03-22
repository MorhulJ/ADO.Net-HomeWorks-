using Library.EfCore;

using var context = new GameContext();


//Task 1.1
var newGame1 = new Game { Name = "Cyberpunk 2077", Studio = "CD Projekt Red", Style = "RPG", ReleaseDate = new DateTime(2020, 12, 10), GameMode = "Singleplayer", CopiesSold = 25000000 };
var newGame2 = new Game { Name = "The Witcher 3", Studio = "CD Projekt Red", Style = "RPG", ReleaseDate = new DateTime(2015, 5, 19), GameMode = "Singleplayer", CopiesSold = 25000000 };
var newGame3 = new Game { Name = "GTA V", Studio = "Rockstar", Style = "Action", ReleaseDate = new DateTime(2013, 9, 17), GameMode = "Singleplayer", CopiesSold = 25000000};
var newGame4 = new Game { Name = "GTA IV", Studio = "Rockstar", Style = "Action", ReleaseDate = new DateTime(2008, 3, 30), GameMode = "Singleplayer", CopiesSold = 25000000};
var newGame5 = new Game { Name = "The Witcher", Studio = "CD Projekt Red", Style = "RPG", ReleaseDate = new DateTime(2009, 10, 9), GameMode = "Singleplayer", CopiesSold = 25000000 };

context.Games.AddRange(newGame1, newGame2, newGame3, newGame4, newGame5);
context.SaveChanges();

var games = context.Games.ToList();

ShowGames(games);

Console.WriteLine("Enter game name:");
string name = Console.ReadLine();

var gamesByName = context.Games
    .Where(g => g.Name.Contains(name))
    .ToList();

ShowGames(gamesByName);


Console.WriteLine("Enter game studio:");
string studio = Console.ReadLine();

var gamesByStudio = context.Games
    .Where(g => g.Studio.Contains(studio))
    .ToList();

ShowGames(gamesByStudio);


Console.WriteLine("Enter game name:");
string gameName = Console.ReadLine();

Console.WriteLine("Enter game studio:");
string gameStudio = Console.ReadLine();

var gamesByNameAndStudio = context.Games
    .Where(g => g.Name.Contains(gameName) && g.Studio.Contains(gameStudio))
    .ToList();

ShowGames(gamesByNameAndStudio);


Console.WriteLine("Enter game style:");
string style = Console.ReadLine();

var gamesByStyle = context.Games
    .Where(g => g.Style.Contains(style))
    .ToList();

ShowGames(gamesByStyle);


Console.WriteLine("Enter release date:");
int year = int.Parse(Console.ReadLine());

var gamesByYear = context.Games
    .Where(g => g.ReleaseDate.Year == year)
    .ToList();

ShowGames(gamesByYear);


var singlePlayerGames = context.Games
    .Where(g => g.GameMode == "Singleplayer")
    .ToList();

ShowGames(singlePlayerGames);


var MultiPlayerGames = context.Games
    .Where(g => g.GameMode == "Multiplayer")
    .ToList();

ShowGames(MultiPlayerGames);


var bestGame = context.Games
    .OrderByDescending(g => g.CopiesSold)
    .FirstOrDefault();

ShowGame(bestGame);


var worstGame = context.Games
    .OrderBy(g => g.CopiesSold)
    .FirstOrDefault();

ShowGame(bestGame);


var top3Sold = context.Games
    .OrderByDescending(g => g.CopiesSold)
    .Take(3)
    .ToList();

ShowGames(top3Sold);


var bottom3Sold = context.Games
    .OrderBy(g => g.CopiesSold)
    .Take(3)
    .ToList();

ShowGames(bottom3Sold);


var newGame = new Game { Name = "Cyber Adventure", Studio = "Adventure Studio", Style = "Adventure", ReleaseDate = new DateTime(2026, 3, 22), GameMode = "Singleplayer", CopiesSold = 0 };
context.Games.Add(newGame);
context.SaveChanges();

newGame2.Name = "Cyberpunk 2026";
context.SaveChanges();

foreach (var g in games)
{
    if (g.Name == "GTA V" && g.Studio == "Rockstar")
    {
        Console.WriteLine("You sure? (y/n)");
        string input = Console.ReadLine()!;
        if (input.ToLower() == "y")
        {
            context.Games.Remove(g);
            context.SaveChanges();
            Console.WriteLine("Game deleted!");
        }
        else
        {
            Console.WriteLine("Deleting canceled.");
        }
        break;
    }
}

void ShowGames(List<Game> games)
{
    foreach (var g in games)
    {
        Console.WriteLine($"{g.Name} | {g.Studio} | {g.Style} | {g.ReleaseDate:yyyy-MM-dd}");
    }
}

void ShowGame(Game game)
{
    Console.WriteLine($"{game.Name} | {game.Studio} | {game.Style} | {game.ReleaseDate:yyyy-MM-dd}");
}