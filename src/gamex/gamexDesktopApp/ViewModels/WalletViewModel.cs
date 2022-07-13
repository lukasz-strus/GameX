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

    public ICommand GetWalletCommand { get; }
    public ICommand UpdateTotalCommand { get; }
    public ICommand GoToAccountViewCommand { get; }
    public ICommand BackToGamesCommand { get; }
    public ICommand LogoutCommand { get; }

    public WalletViewModel(IUserService userService,
                           IAccountStore accountStore,
                           IAuthenticator authenticator,
                           IRenavigator gamesRenavigator,
                           IRenavigator loginRenavigator,
                           IRenavigator accountRenavigator)
    {
        ErrorMessageViewModel = new MessageViewModel();

        GetWalletCommand = new GetWalletCommand(this, userService, accountStore);
        UpdateTotalCommand = new UpdateTotalCommand(this, userService, accountStore);
        GoToAccountViewCommand = new RenavigateCommand(accountRenavigator);
        BackToGamesCommand = new RenavigateCommand(gamesRenavigator);
        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);

        GetWalletCommand.Execute(null);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}