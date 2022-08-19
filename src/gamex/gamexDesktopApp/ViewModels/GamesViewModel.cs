using AutoMapper;
using gamexDesktopApp.Commands;
using gamexDesktopApp.Helpers;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.Selected;
using gamexModels;
using gamexServices;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class GamesViewModel : BaseViewModel, IGamesViewModel, IPagesViewModel
{
    private readonly CollectionViewSource collectionViewSource;

    private readonly ISingleGame _singleGame;

    private string _searchPhrase;

    public string SearchPhrase
    {
        get => _searchPhrase;
        set
        {
            _searchPhrase = value;
            OnPropertyChanged();
        }
    }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 15;

    private SortGameBy _sortBy;

    public SortGameBy SortBy
    {
        get => _sortBy;
        set
        {
            _sortBy = value;
            OnPropertyChanged();
        }
    }

    private SortDirection _sortDirection;

    public SortDirection SortDirection
    {
        get => _sortDirection;
        set
        {
            _sortDirection = value;
            OnPropertyChanged();
        }
    }

    private int _totalPages;

    public int TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged();
        }
    }

    private int _itemsFrom;

    public int ItemsFrom
    {
        get => _itemsFrom;
        set
        {
            _itemsFrom = value;
            OnPropertyChanged();
        }
    }

    private int _itemsTo;

    public int ItemsTo
    {
        get => _itemsTo;
        set
        {
            _itemsTo = value;
            OnPropertyChanged();
        }
    }

    private int _totalItemsCount;

    public int TotalItemsCount
    {
        get => _totalItemsCount;
        set
        {
            _totalItemsCount = value;
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

    public Games Games { get; set; } = new();

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    private Game _selected;

    public Game Selected
    {
        get => _selected;
        set
        {
            _selected = value;

            if (value != null)
            {
                _singleGame.Id = Selected.Id;
            }

            OnPropertyChanged();
        }
    }

    public ICollectionView GamesListView { get; }
    public ICommand RefreshGamesCommand { get; }
    public ICommand UpdatePageCommand { get; }
    public ICommand UpdatePageSizeCommand { get; }
    public ICommand WalletViewCommand { get; }
    public ICommand GameViewCommand { get; }
    public ICommand AccountViewCommand { get; }
    public ICommand LibraryViewCommand { get; }
    public ICommand LogoutCommand { get; }

    public GamesViewModel(IGameService gameService,
                          IAccountStore accountStore,
                          ISingleGame singleGame,
                          IAuthenticator authenticator,
                          IRenavigator gameRenavigator,
                          IRenavigator loginRenavigator,
                          IRenavigator accountRenavigator,
                          IRenavigator walletRenavigator,
                          IRenavigator libraryRenavigator,
                          IFileService fileService)
    {
        ErrorMessageViewModel = new();
        _singleGame = singleGame;

        collectionViewSource = new()
        {
            Source = Games.GamesCollection
        };
        GamesListView = collectionViewSource.View;

        RefreshGamesCommand = new GetGamesListCommand<GamesViewModel>(this, gameService, accountStore, fileService);

        UpdatePageCommand = new UpdatePageCommand<GamesViewModel>(this);
        UpdatePageSizeCommand = new UpdatePageSizeCommand<GamesViewModel>(this);
        WalletViewCommand = new RenavigateCommand(walletRenavigator);
        GameViewCommand = new RenavigateCommand(gameRenavigator);
        AccountViewCommand = new RenavigateCommand(accountRenavigator);
        LibraryViewCommand = new RenavigateCommand(libraryRenavigator);

        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);
        RefreshGamesCommand.Execute(null);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}