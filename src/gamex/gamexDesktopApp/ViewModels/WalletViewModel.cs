using gamexDesktopApp.Commands;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexServices;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class WalletViewModel : BaseViewModel
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

    private string _email;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
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

    private decimal _topUp;

    public decimal TopUp
    {
        get => _topUp;
        set
        {
            _topUp = value;
            OnPropertyChanged();
        }
    }

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    public ICommand RefreshWalletCommand { get; }
    public ICommand UpdateTotalCommand { get; }
    public ICommand AccountViewCommand { get; }
    public ICommand GamesViewCommand { get; }
    public ICommand LogoutCommand { get; }

    public WalletViewModel(IUserService userService,
                           IAccountStore accountStore,
                           IAuthenticator authenticator,
                           IRenavigator gamesRenavigator,
                           IRenavigator loginRenavigator,
                           IRenavigator accountRenavigator)
    {
        ErrorMessageViewModel = new MessageViewModel();

        RefreshWalletCommand = new GetWalletCommand(this, userService, accountStore);
        UpdateTotalCommand = new UpdateTotalCommand(this, userService, accountStore);
        AccountViewCommand = new RenavigateCommand(accountRenavigator);
        GamesViewCommand = new RenavigateCommand(gamesRenavigator);
        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);

        RefreshWalletCommand.Execute(null);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}