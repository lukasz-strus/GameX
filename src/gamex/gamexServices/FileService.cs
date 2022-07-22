using Flurl.Http;
using System.Drawing.Imaging;

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
        AcceptUntrustedCerts();

        var response = await (_localUrl)
            .WithOAuthBearerToken(token)
            .SetQueryParams(new
            {
                gameId
            })
            .GetBytesAsync();

        var fullPath = $"{localFolderPath}/{gameId}.jpg";

        SaveImage(fullPath, response);
    }

    private void SaveImage(string fullPath, byte[] image)
    {
        DeleteExistingImage(fullPath);

        using (var ms = new MemoryStream(image))
        {
            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                ms.WriteTo(fs);
                fs.Close();
            }
        }
    }

    private void DeleteExistingImage(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private void AcceptUntrustedCerts()
    {
        FlurlHttp.ConfigureClient("https://localhost:5001", client =>
            client.Settings.HttpClientFactory = new UntrustedCertClientFactory());
    }
}