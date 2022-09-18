namespace gamexModels.Tests;

public abstract class ValidatorsBaseTest
{
    protected GamexDbContext _dbContext;

    public ValidatorsBaseTest()
    {
        var builder = new DbContextOptionsBuilder<GamexDbContext>();
        builder.UseInMemoryDatabase("TestDb");

        _dbContext = new GamexDbContext(builder.Options);

        Seed();
    }

    protected abstract void Seed();
}