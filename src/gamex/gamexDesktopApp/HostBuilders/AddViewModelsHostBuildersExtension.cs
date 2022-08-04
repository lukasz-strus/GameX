using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.SelectedGame;
using gamexDesktopApp.State.SelectedUser;
using gamexDesktopApp.ViewModels;
using gamexDesktopApp.ViewModels.Factories;
using gamexServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace gamexDesktopApp.HostBuilders;

public static class AddViewModelsHostBuildersExtension
{
    public static IHostBuilder AddViewModels(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddTransient<MainViewModel>();

            services.AddSingleton<CreateViewModel<AccountViewModel>>(services => () => CreateAccountViewModel(services));
            services.AddSingleton<CreateViewModel<WalletViewModel>>(services => () => CreateWalletViewModel(services));
            services.AddSingleton<CreateViewModel<UsersViewModel>>(services => () => CreateUsersViewModel(services));
            services.AddSingleton<CreateViewModel<UserViewModel>>(services => () => CreateUserViewModel(services));
            services.AddSingleton<CreateViewModel<GameViewModel>>(services => () => CreateGameViewModel(services));
            services.AddSingleton<CreateViewModel<GamesViewModel>>(services => () => CreateGamesViewModel(services));
            services.AddSingleton<CreateViewModel<LoginViewModel>>(services => () => CreateLoginViewModel(services));
            services.AddSingleton<CreateViewModel<RegistrationViewModel>>(services => () => CreateRegistrationViewModel(services));
            services.AddSingleton<CreateViewModel<PasswordReminderViewModel>>(services => () => CreatePasswordReminderViewModel(services));
            services.AddSingleton<CreateViewModel<GamesSalesViewModel>>(services => () => CreateGamesSalesViewModel(services));
            services.AddSingleton<CreateViewModel<GamesAdminViewModel>>(services => () => CreateGamesAdminViewModel(services));
            services.AddSingleton<CreateViewModel<GameAdminViewModel>>(services => () => CreateGameAdminViewModel(services));
            services.AddSingleton<CreateViewModel<LibraryViewModel>>(services => () => CreateLibraryViewModel(services));

            services.AddSingleton<IViewModelFactory, ViewModelFactory>();

            services.AddSingleton<ViewModelDelegateRenavigator<WalletViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<AccountViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<GameViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<GamesViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<RegistrationViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<PasswordReminderViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<GamesSalesViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<GamesAdminViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<GameAdminViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<UsersViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<UserViewModel>>();
            services.AddSingleton<ViewModelDelegateRenavigator<LibraryViewModel>>();
        });

        return host;
    }

    private static LibraryViewModel CreateLibraryViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<WalletViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesViewModel>>());

    private static UserViewModel CreateUserViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IUserService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<ISingleUser>(),
            services.GetRequiredService<IAuthenticationService>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<UsersViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>());

    private static UsersViewModel CreateUsersViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IUserService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<ISingleUser>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<UserViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesAdminViewModel>>());

    private static GameAdminViewModel CreateGameAdminViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IGameService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<ISingleGame>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesAdminViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesSalesViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>(),
            services.GetRequiredService<IFileService>());

    private static GamesAdminViewModel CreateGamesAdminViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IGameService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<ISingleGame>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GameAdminViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<UsersViewModel>>(),
            services.GetRequiredService<IFileService>());

    private static GamesSalesViewModel CreateGamesSalesViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IGameService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<ISingleGame>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GameAdminViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesViewModel>>(),
            services.GetRequiredService<IFileService>());

    private static WalletViewModel CreateWalletViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IUserService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>());

    private static AccountViewModel CreateAccountViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IUserService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<IAuthenticationService>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<WalletViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesAdminViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesSalesViewModel>>());

    private static GameViewModel CreateGameViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IGameService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<ISingleGame>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<IUserService>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<WalletViewModel>>(),
            services.GetRequiredService<IFileService>());

    private static GamesViewModel CreateGamesViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IGameService>(),
            services.GetRequiredService<IAccountStore>(),
            services.GetRequiredService<ISingleGame>(),
            services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GameViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<AccountViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<WalletViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LibraryViewModel>>(),
            services.GetRequiredService<IFileService>());

    private static LoginViewModel CreateLoginViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<GamesViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<RegistrationViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<PasswordReminderViewModel>>());

    private static RegistrationViewModel CreateRegistrationViewModel(IServiceProvider services) =>
        new(services.GetRequiredService<IAuthenticator>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
            services.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>());

    public static PasswordReminderViewModel CreatePasswordReminderViewModel(IServiceProvider service) =>
        new(service.GetRequiredService<IAuthenticator>(),
            service.GetRequiredService<ViewModelDelegateRenavigator<RegistrationViewModel>>());
}