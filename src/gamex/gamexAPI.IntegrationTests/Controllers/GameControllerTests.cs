using FluentAssertions;
using gamexModels;
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
    public async Task Create_WithValidQueryParams_ReturnsBadRequest(string name, string description, decimal price)
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
    public async Task Create_WithQueryParams_ReturnsCreatedRequest(string name, string description, decimal price)
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
    public async Task GetAllGames_WithWithQueryParam_ReturnsOk(string queryParams)
    {
        var response = await Client.GetAsync("api/game?" + queryParams);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("pageSize=1&pageNumber=1")]
    [InlineData("pageSize=-1&pageNumber=1")]
    [InlineData("pageSize=5&pageNumber=-1")]
    public async Task GetAllGames_WithQueryParam_ReturnsBadRequest(string queryParams)
    {
        var response = await Client.GetAsync("api/game?" + queryParams);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion GetAll

    #region Get

    [Theory]
    [InlineData(1)]
    public async Task Get_WithIdParam_ReturnOk(int id)
    {
        var response = await Client.GetAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(100000000)]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Get_WithIdParam_ReturnNotFound(int id)
    {
        var response = await Client.GetAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion Get

    #region Delete

    [Theory]
    [InlineData(2)]
    public async Task Delete_WithIdParameter_ReturnNoContent(int id)
    {
        var response = await Client.DeleteAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [InlineData(100000000)]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Delete_WithIdParameter_ReturnNotFound(int id)
    {
        var response = await Client.DeleteAsync("api/game/" + id);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion Delete

    #region Update

    [Theory]
    [InlineData(1, "", "Test Description", "100")]
    [InlineData(1, "Test Name 1", "", "120")]
    [InlineData(1, "Test Name 2", "Test Description", null)]
    public async Task Update_WithQueryParams_ReturnsOk(int id, string name, string description, string price)
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

    [Theory]
    [InlineData(1, "Test Name", "Test Description", "120")]
    public async Task Update_WithTheSameName_ReturnsBadRequest(int id, string name, string description, string price)
    {
        var model = new UpdateGameDto()
        {
            Name = name,
            Description = description,
            Price = Convert.ToDecimal(price)
        };

        var json = JsonConvert.SerializeObject(model);
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        await Client.PutAsync($"api/game/{id}", httpContent);
        var response = await Client.PutAsync($"api/game/{id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion Update

    #region GetSerialKey

    [Theory]
    [InlineData(1, 1)]
    public async Task GetSerialKey_WithIdParams_ReturnOk(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.RequestMessage.Should().NotBe(null);
    }

    [Theory]
    [InlineData(2, 1)]
    public async Task GetSerialKey_WithIdParams_ReturnNotAcceptable(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
    }

    [Theory]
    [InlineData(4, 1)]
    [InlineData(1, 10)]
    public async Task GetSerialKey_WithIdParams_ReturnNotFound(int userId, int gameId)
    {
        var response = await Client.GetAsync($"api/game/{userId}/{gameId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion GetSerialKey

}