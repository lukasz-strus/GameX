using Flurl.Http;
using gamexModels;
using System.Drawing.Imaging;
using System.Net;

namespace gamexServices;

public interface IFileService
{
    Task<ImageDto> GetGameImage(string token, int gameId);
}

public class FileService : IFileService
{
    private readonly string _baseUrl = "https://gamex-api-app.azurewebsites.net/file";

    private readonly string _localUrl = "https://localhost:5001/file";

    public async Task<ImageDto> GetGameImage(string token, int gameId)
    {
        AcceptUntrustedCerts();

        var response = await (_baseUrl + $"/{gameId}")
            .WithOAuthBearerToken(token)
            .AllowAnyHttpStatus()
            .GetAsync();

        if (response.StatusCode == (int)HttpStatusCode.NotFound) //TODO Zrobić to innych serwisach + API
        {
            return null;
        }

        var result = await response.GetJsonAsync<ImageDto>();

        return result;
    }

    private void AcceptUntrustedCerts()
    {
        FlurlHttp.ConfigureClient("https://localhost:5001", client =>
            client.Settings.HttpClientFactory = new UntrustedCertClientFactory());
    }
}