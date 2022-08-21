using gamexEntities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace gamexAPI.IntegrationTests.Controllers;

public abstract class BaseControllerTests
{
    protected HttpClient _client;

    public BaseControllerTests()
    {
        _client = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services
                        .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<GamexDbContext>));

                    services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));

                    services
                        .AddDbContext<GamexDbContext>(options => options.UseInMemoryDatabase("gamexDb"));
                });
            })
            .CreateClient();
    }
}