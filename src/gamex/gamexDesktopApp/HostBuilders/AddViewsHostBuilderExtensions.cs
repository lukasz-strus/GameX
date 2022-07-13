using gamexDesktopApp.ViewModels;
using gamexDesktopApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace gamexDesktopApp.HostBuilders;

public static class AddViewsHostBuilderExtensions
{
    public static IHostBuilder AddViews(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
        });

        return host;
    }
}