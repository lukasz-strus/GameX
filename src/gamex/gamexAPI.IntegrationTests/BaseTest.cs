using Microsoft.AspNetCore.TestHost;
using Xunit.Abstractions;

namespace gamexAPI.IntegrationTests.Controllers;

public class BaseTest
{
    public ITestOutputHelper Output { get; }

    public TestWebAppFactory<Program> Factory { get; }
    public HttpClient Client { get; }
    public TestServer Server { get; }
    public List<string> Logs { get; }

    public BaseTest(ITestOutputHelper output)
    {
        Output = output;

        // Factory
        Factory = new TestWebAppFactory<Program>(output);

        // HttpClient
        Client = Factory.CreateClient();

        // Test Server
        Server = Factory.Server;
    }
}