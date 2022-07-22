using gamexDesktopApp.Helpers;
using gamexDesktopApp.HostBuilders;
using gamexDesktopApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace gamexDesktopApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = CreateHostBuilder().Build();
    }

    public static IHostBuilder CreateHostBuilder(string[] args = null) =>
        Host.CreateDefaultBuilder(args)
        .AddServices()
        .AddStores()
        .AddViewModels()
        .AddViews();

    protected override void OnStartup(StartupEventArgs e)
    {
        _host.Start();

        Window window = _host.Services.GetRequiredService<MainWindow>();
        window.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        FileHelper.DeleteGamesImages();

        await _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}