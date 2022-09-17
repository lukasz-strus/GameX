using gamexAPI.Services;
using gamexEntities;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace gamexAPI.IntegrationTests;

public class TestWebAppFactory<TEntryPoint> : WebApplicationFactory<Program>
        where TEntryPoint : Program
{
    public ITestOutputHelper Output { get; set; }
    public Mock<IUserService> UserService = new();

    public TestWebAppFactory([NotNull] ITestOutputHelper output)
    {
        Output = output;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptions = services
                .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<GamexDbContext>));

            services.Remove(dbContextOptions);

            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddSingleton(UserService.Object);

            services.AddMvc(option => option.Filters.Add(new FakeUserFilter()))
                    .AddApplicationPart(typeof(Program).Assembly);

            services
             .AddDbContext<GamexDbContext>(options => options.UseInMemoryDatabase("GamexDb"));
        });
    }
}