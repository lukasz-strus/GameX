using FluentAssertions;
using gamexModels;
using Moq;
using System.Net;

namespace gamexServices.Tests;

public class GameServiceTests
{
    private Mock<gamexAPI.Services.IGameService> _mockGameService = new();
    private Mock<gamexAPI.Services.IUserService> _mockUserService = new();
    private GameService _gameService = new();

    [Fact]
    public void CreateTest()
    {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact]
    public void GetAllTest()
    {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact]
    public async void GetTest()
    {
        _mockGameService
            .Setup(u => u.Get(It.IsAny<int>()))
            .Returns(new GameDto());

        var response = await _gameService.Get("token", 1);

        response.Should().NotBeNull();
    }

    [Fact]
    public void DeleteTest()
    {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact]
    public void UpdateTest()
    {
        Assert.True(false, "This test needs an implementation");
    }

    [Fact]
    public void GetSerialKeyTest()
    {
        Assert.True(false, "This test needs an implementation");
    }
}