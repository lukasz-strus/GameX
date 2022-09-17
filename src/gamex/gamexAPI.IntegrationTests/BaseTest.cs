namespace gamexAPI.IntegrationTests;

public class BaseTest
{
    public ITestOutputHelper Output { get; }

    public TestWebAppFactory<Program> Factory { get; }
    public HttpClient Client { get; }
    public TestServer Server { get; }
    public List<string> Logs { get; }

    public BaseTest(ITestOutputHelper output)
    {
        Output = output;

        // Factory
        Factory = new TestWebAppFactory<Program>(output);

        // HttpClient
        Client = Factory.CreateClient();

        // Test Server
        Server = Factory.Server;
    }

    protected void Seed(Game game = null, User user = null, Image image = null)
    {
        var scopeFactory = Factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<GamexDbContext>();

        if (game != null)
            _dbContext.Games.Add(game);
        if (user != null)
            _dbContext.Users.Add(user);
        if (image != null)
            _dbContext.Images.Add(image);

        _dbContext.SaveChanges();
    }
}