﻿using gamexDesktopApp.Commands;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.Selected;
using gamexModels;
using gamexServices;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class GamesSalesViewModel : BaseViewModel, IGamesViewModel, IPagesViewModel
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

    public ICommand GameViewCommand { get; }
    public ICommand AddNewGameCommand { get; }
    public ICommand AccountViewCommand { get; }
    public ICommand DeleteGameCommand { get; }

    public ICommand LogoutCommand { get; }
    public ICommand GamesViewCommand { get; }

    public GamesSalesViewModel(IGameService gameService,
                              IAccountStore accountStore,
                              ISingleGame singleGame,
                              IAuthenticator authenticator,
                              IRenavigator gameAdminRenavigator,
                              IRenavigator loginRenavigator,
                              IRenavigator accountRenavigator,
                              IRenavigator gamesRenavigator,
                              IFileService fileService)
    {
        ErrorMessageViewModel = new();
        _singleGame = singleGame;

        collectionViewSource = new()
        {
            Source = Games.GamesCollection
        };
        GamesListView = collectionViewSource.View;

        RefreshGamesCommand = new GetGamesListCommand<GamesSalesViewModel>(this, gameService, accountStore, fileService);
        RefreshGamesCommand.Execute(null);

        UpdatePageCommand = new UpdatePageCommand<GamesSalesViewModel>(this);
        UpdatePageSizeCommand = new UpdatePageSizeCommand<GamesSalesViewModel>(this);

        GameViewCommand = new RenavigateCommand(gameAdminRenavigator);
        AddNewGameCommand = new AddNewCommand(gameAdminRenavigator, singleGame);
        AccountViewCommand = new RenavigateCommand(accountRenavigator);
        DeleteGameCommand = new DeleteCommand(this, singleGame, gameService, accountStore);

        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);
        GamesViewCommand = new RenavigateCommand(gamesRenavigator);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}