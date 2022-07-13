using gamexDesktopApp.Commands;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class LibraryViewModel : BaseViewModel
{
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

    public ICommand GoToWalletCommand { get; }
    public ICommand GoToAccountViewCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand GoToGamesViewCommand { get; }

    public LibraryViewModel(IAuthenticator authenticator,
                            IRenavigator walletRenavigator,
                            IRenavigator accountRenavigator,
                            IRenavigator loginRenavigator,
                            IRenavigator gamesRenavigator)
    {
        GoToWalletCommand = new RenavigateCommand(walletRenavigator);
        GoToAccountViewCommand = new RenavigateCommand(accountRenavigator);
        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);
        GoToGamesViewCommand = new RenavigateCommand(gamesRenavigator);
    }
}