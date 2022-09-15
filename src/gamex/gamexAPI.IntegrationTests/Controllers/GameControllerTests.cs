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
using Xunit.Abstractions;

namespace gamexAPI.IntegrationTests.Controllers;

[Collection(Constants.TEST_COLLECTION)]
public class GameControllerTests : BaseTest
{
    public GameControllerTests(ITestOutputHelper output) : base(output)
    {
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

        var response = await Client.PostAsync("api/game", httpContent);

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

        var response = await Client.GetAsync("api/game?" + queryParams);

        //assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}