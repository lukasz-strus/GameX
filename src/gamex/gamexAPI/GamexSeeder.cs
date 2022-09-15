namespace gamexAPI;

public class GamexSeeder
{
    private readonly GamexDbContext _dbContext;

    public GamexSeeder(GamexDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
        {
            if (_dbContext.Database.IsRelational())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }
            }

            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                _dbContext.SaveChanges();
            }

            if (!_dbContext.Users.Any())
            {
                var users = GetUsers();

                _dbContext.Users.AddRange(users);

                _dbContext.SaveChanges();
            }

            if (!_dbContext.Games.Any())
            {
                var games = GetGames();
                _dbContext.Games.AddRange(games);
                _dbContext.SaveChanges();
            }

            //if (!_dbContext.Images.Any())
            //{
            //    var images = GetImages();
            //    _dbContext.Images.AddRange(images);
            //    _dbContext.SaveChanges();
            //}
        }
    }

    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>()
        {
            new Role()
            {
                Name = "User"
            },
            new Role()
            {
                Name = "Seller"
            },
            new Role()
            {
                Name = "Admin"
            }
        };

        return roles;
    }

    private IEnumerable<Game> GetGames()
    {
        var games = new List<Game>()
        {
            new Game()
            {
                Name = "The Witcher 3: Wild Hunt",
                Description = "Action role-playing video game developed by Polish developer CD Projekt Red",
                Price = 120m
            },
            new Game()
            {
                Name = "Call of Duty: Modern Warfare",
                Description = "First-person shooter game developed by Infinity Ward and published by Activision",
                Price = 250m
            }
        };
        return games;
    }

    private IEnumerable<User> GetUsers()
    {
        var users = new List<User>()
        {
            new User()
            {
                Login="Admin",
                Email="admin@gmail.com",
                PasswordHash="AQAAAAEAACcQAAAAEFmpjHfAfOpJDWGnKtwGnuSHmNYBrULw1uRaB2gWdnOMCHOfvurFdo8gXMES7Sh0CQ==", //admin123456789
                Total = 1000m,
                RoleId = 3
            },
            new User()
            {
                Login="User",
                Email="user@gmail.com",
                PasswordHash="AQAAAAEAACcQAAAAEFmpjHfAfOpJDWGnKtwGnuSHmNYBrULw1uRaB2gWdnOMCHOfvurFdo8gXMES7Sh0CQ==", //admin123456789
                Total = 0,
                RoleId = 1
            }
        };

        return users;
    }

    private IEnumerable<Image> GetImages()
    {
        var images = new List<Image>()
        {
            new Image()
            {
                ImageStream = GetImageFile("1"),
                GameId = 1,
                Extension = GetImageExtension("1")
            },
            new Image()
            {
                ImageStream = GetImageFile("2"),
                GameId = 2,
                Extension = GetImageExtension("2")
            }
        };

        return images;
    }

    private static byte[] GetImageFile(string id)
    {
        string filePath = $@"D:\Microsoft Visual Studio\source\gamex\src\gamex\gamexAPI\PrivateFiles\Games\{id}.jpg";
        return File.ReadAllBytes(filePath);
    }

    private static string GetImageExtension(string id)
    {
        string filePath = $@"D:\Microsoft Visual Studio\source\gamex\src\gamex\gamexAPI\PrivateFiles\Games\{id}.jpg";
        return Path.GetExtension(filePath);
    }
}