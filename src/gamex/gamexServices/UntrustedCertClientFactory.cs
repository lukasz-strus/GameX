using Flurl.Http.Configuration;

namespace gamexServices;

internal class UntrustedCertClientFactory : DefaultHttpClientFactory
{
    public override HttpMessageHandler CreateMessageHandler() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    };
}