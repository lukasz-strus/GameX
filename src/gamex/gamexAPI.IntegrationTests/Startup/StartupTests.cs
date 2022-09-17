using FluentAssertions;
using gamexAPI.IntegrationTests.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace gamexAPI.IntegrationTests.Startup;

[Collection(Constants.TEST_COLLECTION)]
public class StartupTests : BaseTest
{
    private readonly List<Type> _controllerTypes;
    private readonly WebApplicationFactory<Program> _factory;

    public StartupTests(ITestOutputHelper output) : base(output)
    {
        _controllerTypes = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
            .ToList();

        _factory = Factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                _controllerTypes.ForEach(c => services.AddScoped(c));
            });
        });
    }

    [Fact]
    public void ConfigureServices_ForControllers_RegistersAllDependencies()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();

        _controllerTypes.ForEach(t =>
        {
            var controller = scope.ServiceProvider.GetService(t);
            controller.Should().NotBeNull();
        });
    }
}