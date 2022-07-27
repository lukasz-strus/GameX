using Flurl.Http;
using System.Drawing.Imaging;

namespace gamexServices;

public interface IFileService
{
    Task<byte[]> GetGameImage(string token, int gameId);
}

public class FileService : IFileService
{
    private readonly string _baseUrl = "https://gamex-api-app.azurewebsites.net/file";

    private readonly string _localUrl = "https://localhost:5001/file";

    public async Task<byte[]> GetGameImage(string token, int gameId)
    {
        AcceptUntrustedCerts();

        var response = await (_localUrl)
            .WithOAuthBearerToken(token)
            .SetQueryParams(new
            {
                gameId
            })
            .GetBytesAsync();

        return response;
    }

    private void AcceptUntrustedCerts()
    {
        FlurlHttp.ConfigureClient("https://localhost:5001", client =>
            client.Settings.HttpClientFactory = new UntrustedCertClientFactory());
    }
}