using gamexDesktopApp.Commands;
using gamexDesktopApp.Helpers;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.SelectedGame;
using gamexServices;
using System.IO;
using System.Reflection;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

/// <summary>
/// GameViewModel Class
/// </summary>
public class GameViewModel : BaseViewModel, IGameViewModel
{
    private int _id;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            Source = ImageSourceHelper.SetSource(_id);
            OnPropertyChanged();
        }
    }

    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _description;

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    private decimal _price;

    public decimal Price
    {
        get => _price;
        set
        {
            _price = value;
            OnPropertyChanged();
        }
    }

    private string _serialKey;

    public string SerialKey
    {
        get => _serialKey;
        set
        {
            _serialKey = value;
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

    private string _source;

    public string Source
    {
        get => _source;
        set
        {
            _source = value;
            OnPropertyChanged();
        }
    }

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    public ICommand GetGameCommand { get; }
    public ICommand BuyGameCommand { get; }
    public ICommand BackToGamesCommand { get; }
    public ICommand GoToWalletCommand { get; }
    public ICommand GoToAccountViewCommand { get; }
    public ICommand LogoutCommand { get; }

    public GameViewModel(IGameService gameService,
                         IAccountStore accountStore,
                         ISingleGame singleGame,
                         IAuthenticator authenticator,
                         IUserService userService,
                         IRenavigator gamesRenavigator,
                         IRenavigator loginRenavigator,
                         IRenavigator accountRenavigator,
                         IRenavigator walletRenavigator)
    {
        ErrorMessageViewModel = new MessageViewModel();

        GetGameCommand = new GetGameCommand<GameViewModel>(this, gameService, accountStore, singleGame);
        GetGameCommand.Execute(null);
        BuyGameCommand = new BuyGameCommand(this, gameService, accountStore, userService);
        BackToGamesCommand = new RenavigateCommand(gamesRenavigator);
        GoToWalletCommand = new RenavigateCommand(walletRenavigator);
        GoToAccountViewCommand = new RenavigateCommand(accountRenavigator);
        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}