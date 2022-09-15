using FluentAssertions;
using gamexEntities;
using gamexModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit.Abstractions;

namespace gamexAPI.IntegrationTests.Controllers;

[Collection(Constants.TEST_COLLECTION)]
public class GameControllerTests : BaseTest
{
    public GameControllerTests(ITestOutputHelper output) : base(output)
    {
    }

    #region Create

    [Theory]
    [InlineData("", "Test Description", 100)]
    [InlineData("Test Name", "", null)]
    public async Task Create_WithIncorrectQueryParams_ReturnsBadRequest(string name, string description, decimal price)
    {
        var model = new CreateGameDto()
        {
            Name = name,
            Description = description,
            Price = price
        };
        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        var response = await Client.PostAsync("api/game", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("Test Name 1", "Test Description", 100)]
    [InlineData("Test Name 2", "", 100)]
    [InlineData("Test Name 3", "", 0)]
    public async Task Create_WithTheSameName_ReturnsBadRequest(string name, string description, decimal price)
    {
        var model = new CreateGameDto()
        {
            Name = name,
            Description = description,
            Price = price
        };
        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        await Client.PostAsync("api/game", httpContent);

        var response = await Client.PostAsync("api/game", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("Test Name", "Test Description", 100)]
    public async Task Create_WithCorrectQueryParams_ReturnsCreated(string name, string description, decimal price)
    {
        var model = new CreateGameDto()
        {
            Name = name,
            Description = description,
            Price = price
        };
        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        var response = await Client.PostAsync("api/game", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    #endregion Create

    #region GetAll

    [Theory]
    [InlineData("pageSize=5&pageNumber=1")]
    [InlineData("pageSize=10&pageNumber=2")]
    [InlineData("pageSize=15&pageNumber=3")]
    public async Task GetAllGames_WithCorrectQueryParam_ReturnsOk(string queryParams)
    {
        var response = await Client.GetAsync("api/game?" + queryParams);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("pageSize=1&pageNumber=1")]
    [InlineData("pageSize=-1&pageNumber=1")]
    [InlineData("pageSize=5&pageNumber=-1")]
    public async Task GetAllGames_WithIncorrectQueryParam_ReturnsBadRequest(string queryParams)
    {
        var response = await Client.GetAsync("api/game?" + queryParams);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion GetAll

    #region Get

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Get_ForExistId_ReturnOk(int id)
    {
        var response = await Client.GetAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(100000000)]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Get_ForNonExistentId_ReturnNotFound(int id)
    {
        var response = await Client.GetAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion Get

    #region Delete

    [Fact]
    public async Task Delete_ForExistGame_ReturnNoContent()
    {
        var game = new Game()
        {
            Id = 10,
            Name = "Test Delete",
            Description = "Test Delete",
            Price = 100
        };

        SeedGame(game);

        var response = await Client.DeleteAsync("api/game/" + game.Id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData(100000000)]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Delete_ForNonExistentGame_ReturnNotFound(int id)
    {
        var response = await Client.DeleteAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private void SeedGame(Game game)
    {
        var scopeFactory = Factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<GamexDbContext>();

        _dbContext.Games.Add(game);
        _dbContext.SaveChanges();
    }

    #endregion Delete

    #region Update

    [Theory]
    [InlineData(1, "", "Test Description", "100")]
    [InlineData(1, "Test 1", "", "120")]
    [InlineData(1, "Test 2", "Test Description", null)]
    public async Task Update_ForCorrectQueryParams_ReturnsOk(int id, string name, string description, string price)
    {
        var model = new UpdateGameDto();

        if (name != null) model.Name = name;
        if (description != null) model.Description = description;
        if (price != null) model.Price = Convert.ToDecimal(price);

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        var response = await Client.PutAsync($"api/game/{id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_ForTheSameName_ReturnsBadRequest()
    {
        var model = new UpdateGameDto()
        {
            Name = "Test",
            Description = "Test",
            Price = 120m
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        await Client.PutAsync($"api/game/2", httpContent);
        var response = await Client.PutAsync($"api/game/2", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_ForNonExistentId_ReturnsNotFound()
    {
        var model = new UpdateGameDto()
        {
            Name = "Test",
            Description = "Test",
            Price = 120m
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        var response = await Client.PutAsync($"api/game/100", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion Update

    #region GetSerialKey

    [Theory]
    [InlineData(1, 1)]
    public async Task GetSerialKey_ForCorrectId_ReturnOk(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.RequestMessage.Should().NotBe(null);
    }

    [Theory]
    [InlineData(2, 1)]
    public async Task GetSerialKey_ForIncorrectParams_ReturnNotAcceptable(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    [Theory]
    [InlineData(4, 1)]
    [InlineData(1, 10)]
    public async Task GetSerialKey_ForNonExistentId_ReturnNotFound(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion GetSerialKey
}