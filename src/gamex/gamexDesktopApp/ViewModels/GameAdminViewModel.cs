using gamexDesktopApp.Commands;
using gamexDesktopApp.Helpers;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.SelectedGame;
using gamexServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class GameAdminViewModel : BaseViewModel, IGameViewModel
{
    private int _id;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            Source = SourceHelper.SetSource(_id);
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
    public ICommand UpdateGameCommand { get; }
    public ICommand BackToGamesCommand { get; }
    public ICommand GoToAccountViewCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand LoadImageCommand { get; }

    public GameAdminViewModel(IGameService gameService,
                             IAccountStore accountStore,
                             ISingleGame singleGame,
                             IAuthenticator authenticator,
                             IRenavigator gamesAdminRenavigator,
                             IRenavigator gamesSalesRenavigator,
                             IRenavigator loginRenavigator,
                             IRenavigator accountRenavigator,
                             IFileService fileService)
    {
        ErrorMessageViewModel = new MessageViewModel();

        GetGameCommand = new GetGameCommand<GameAdminViewModel>(this, gameService, accountStore, singleGame, fileService);
        GetGameCommand.Execute(null);
        UpdateGameCommand = new UpdateGameCommand(this, gameService, accountStore, singleGame);
        BackToGamesCommand = new BackToGamesAdminCommand(gamesAdminRenavigator, gamesSalesRenavigator, accountStore);
        GoToAccountViewCommand = new RenavigateCommand(accountRenavigator);
        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);
        LoadImageCommand = new LoadImageCommand(this);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}