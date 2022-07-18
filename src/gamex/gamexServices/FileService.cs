using Flurl.Http;

namespace gamexServices;

public interface IFileService
{
    Task GetGameImage(string token, int gameId, string localFolderPath);
}

public class FileService : IFileService
{
    private readonly string _baseUrl = "https://gamex-api-app.azurewebsites.net/file";

    private readonly string _localUrl = "https://localhost:5001/file";

    public async Task GetGameImage(string token, int gameId, string localFolderPath)
    {
        var response = await (_baseUrl)
            .WithOAuthBearerToken(token)
            .SetQueryParams(new
            {
                gameId
            })
            .GetStreamAsync();

        using (var stream = new FileStream(localFolderPath, FileMode.Create))
        {
            response.CopyTo(stream);
        }
    }
}