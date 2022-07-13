using gamexDesktopApp.Commands;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexServices;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class AccountViewModel : BaseViewModel, IPasswordViewModel
{
    private string _login;

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
        }
    }

    private string _mail;

    public string Email
    {
        get => _mail;
        set
        {
            _mail = value;
            OnPropertyChanged();
        }
    }

    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    private string _confirmPassword;

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged();
        }
    }

    private Role _role;

    public Role Role
    {
        get => _role;
        set
        {
            _role = value;
            OnPropertyChanged();
        }
    }

    private decimal _total;

    public decimal Total
    {
        get => _total;
        set
        {
            _total = value;
            OnPropertyChanged();
        }
    }

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    public ICommand GetAccountCommand { get; }
    public ICommand ChangePasswordCommand { get; }
    public ICommand GoToWalletCommand { get; }
    public ICommand BackToGamesCommand { get; }
    public ICommand LogoutCommand { get; }

    public ICommand GoToGamesAdminCommand { get; }

    public bool IsAdmin() => Role == Role.Admin;

    public bool IsSeller() => Role == Role.Seller;

    public AccountViewModel(IUserService userService,
                            IAccountStore accountStore,
                            IAuthenticationService authenticationService,
                            IAuthenticator authenticator,
                            IRenavigator gamesRenavigator,
                            IRenavigator loginRenavigator,
                            IRenavigator walletRenavigator,
                            IRenavigator gamesAdminRenavigator,
                            IRenavigator gamesSellerRenavigator)
    {
        ErrorMessageViewModel = new MessageViewModel();

        GetAccountCommand = new GetAccountCommand(this, userService, accountStore);
        ChangePasswordCommand = new ChangePasswordCommand(this, authenticationService, accountStore);

        GoToWalletCommand = new RenavigateCommand(walletRenavigator);
        BackToGamesCommand = new RenavigateCommand(gamesRenavigator);
        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);

        GoToGamesAdminCommand = new GoToGamesAdminCommand(accountStore, gamesAdminRenavigator, gamesSellerRenavigator);

        GetAccountCommand.Execute(null);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}