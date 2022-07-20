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
        FlurlHttp.ConfigureClient("https://localhost:5001", client =>
            client.Settings.HttpClientFactory = new UntrustedCertClientFactory());

        var response = await (_localUrl)
            .WithOAuthBearerToken(token)
            .SetQueryParams(new
            {
                gameId
            })
            .DownloadFileAsync(Path.Combine(localFolderPath));
    }
}