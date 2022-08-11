using gamexDesktopApp.Commands;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.SelectedUser;
using gamexModels;
using gamexServices;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class UsersViewModel : BaseViewModel, IPagesViewModel
{
    private readonly CollectionViewSource collectionViewSource;
    private readonly ISingleUser _singleUser;

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
    public int PageSize { get; set; } = 5;

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

    public Users Users { get; set; } = new();

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    private User _selected;

    public User Selected
    {
        get => _selected;
        set
        {
            _selected = value;

            if (value != null)
            {
                _singleUser.Id = Selected.Id;
            }

            OnPropertyChanged();
        }
    }

    public ICollectionView UsersListView { get; }
    public ICommand ViewListCommand { get; }
    public ICommand UpdatePageCommand { get; }
    public ICommand UpdatePageSizeCommand { get; }
    public ICommand GoToUserViewCommand { get; }
    public ICommand AddNewUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand GoToAccountViewCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand BackToGamesCommand { get; }
    public ICommand GoToGamesAdminCommand { get; }

    public UsersViewModel(IUserService userService,
                          IAccountStore accountStore,
                          ISingleUser singleUser,
                          IAuthenticator authenticator,
                          IRenavigator userRenavigator,
                          IRenavigator loginRenavigator,
                          IRenavigator accountRenavigator,
                          IRenavigator gamesRenavigator,
                          IRenavigator gamesAdminRenavigator)
    {
        ErrorMessageViewModel = new();
        _singleUser = singleUser;

        collectionViewSource = new()
        {
            Source = Users.UsersCollection
        };
        UsersListView = collectionViewSource.View;

        ViewListCommand = new GetUsersListCommand(this, userService, accountStore);
        UpdatePageCommand = new UpdatePageCommand<UsersViewModel>(this);
        UpdatePageSizeCommand = new UpdatePageSizeCommand<UsersViewModel>(this);

        GoToUserViewCommand = new RenavigateCommand(userRenavigator);
        AddNewUserCommand = new AddNewUserCommand(userRenavigator, singleUser);
        DeleteUserCommand = new DeleteUserCommand(this, singleUser, userService, accountStore);

        GoToAccountViewCommand = new RenavigateCommand(accountRenavigator);

        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);

        BackToGamesCommand = new RenavigateCommand(gamesRenavigator);

        GoToGamesAdminCommand = new RenavigateCommand(gamesAdminRenavigator);

        ViewListCommand.Execute(null);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}