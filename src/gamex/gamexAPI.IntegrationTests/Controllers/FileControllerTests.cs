using gamexAPI.IntegrationTests.Data;
using gamexAPI.IntegrationTests.Helpers;

namespace gamexAPI.IntegrationTests.Controllers;

[Collection(Constants.TEST_COLLECTION)]
public class FileControllerTests : BaseTest
{
    public FileControllerTests(ITestOutputHelper output) : base(output)
    {
    }

    #region Get

    [Fact]
    public async Task Get_ForExistGameId_ReturnOk()
    {
        var gameId = 1;
        var image = new Image()
        {
            Id = 1000,
            ImageStream = new byte[] { 1, 2, 3 },
            GameId = gameId,
            Extension = ".jpg"
        };

        Seed(null, null, image);

        var response = await Client.GetAsync("file/" + gameId);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(100000000)]
    [InlineData(-10)]
    [InlineData(0)]
    public async Task Get_ForNonExistentGameId_ReturnNotFound(int gameId)
    {
        var response = await Client.GetAsync("file/" + gameId);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion Get
}

//TODO Create, Delete, GetGameImages