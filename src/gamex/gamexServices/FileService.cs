using Flurl.Http;
using System.Drawing.Imaging;

namespace gamexServices;

public interface IFileService
{
    void GetGameImage(string token, int gameId, string localFolderPath);
}

public class FileService : IFileService
{
    private readonly string _baseUrl = "https://gamex-api-app.azurewebsites.net/file";

    private readonly string _localUrl = "https://localhost:5001/file";

    public async void GetGameImage(string token, int gameId, string localFolderPath)
    {
        await GetImages(token, gameId, localFolderPath);
    }

    private async Task GetImages(string token, int gameId, string localFolderPath)
    {
        FlurlHttp.ConfigureClient("https://localhost:5001", client =>
            client.Settings.HttpClientFactory = new UntrustedCertClientFactory());

        var response = await (_localUrl)
            .WithOAuthBearerToken(token)
            .SetQueryParams(new
            {
                gameId
            })
            .GetBytesAsync();

        var fullPath = $"{localFolderPath}/{gameId}.jpg";

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        using (var ms = new MemoryStream(response))
        {
            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                ms.WriteTo(fs);
                fs.Close();
            }
        }
    }
}