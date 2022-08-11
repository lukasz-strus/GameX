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
        await _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }

    //TODO CLEAN CODE
    //TODO CC2. Rozdzielić Metody
    //TODO CC3. Wydzielić metody pomocnicze dla komend
    //TODO CC4. Generyczność komend
    //TODO CC5. Wyczyścic dokładnie pod kolejną gałąź - DESING!!!
}