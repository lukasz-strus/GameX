using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.SelectedGame;
using gamexDesktopApp.State.SelectedUser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace gamexDesktopApp.HostBuilders;

public static class AddStoresHostBuilderExtensions
{
    public static IHostBuilder AddStores(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<ISingleGame, SingleGame>();
            services.AddSingleton<ISingleUser, SingleUser>();
        });

        return host;
    }
}