using FluentAssertions;
using gamexEntities;
using gamexModels;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace gamexAPI.IntegrationTests.Controllers;

public class GameControllerTests
{
    protected HttpClient _client;

    public GameControllerTests()
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
                     .AddDbContext<GamexDbContext>(options => options.UseInMemoryDatabase("GamexDb"));
                });
            }).CreateClient();
    }

    [Fact]
    public async Task Create_WithValidModel_ReturnsCreated()
    {
        //arrange

        var model = new CreateGameDto()
        {
            Name = "Test Name",
            Description = "Test Description",
            Price = 120m
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        //act

        var response = await _client.PostAsync("api/game", httpContent);

        //assert

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    [Theory]
    [InlineData("pageSize=5&pageNumber=1")]
    [InlineData("pageSize=10&pageNumber=2")]
    [InlineData("pageSize=15&pageNumber=3")]
    public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParams)
    {
        //act

        var response = await _client.GetAsync("api/game?" + queryParams);

        //assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}