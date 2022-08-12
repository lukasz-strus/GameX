using gamexDesktopApp.Commands;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.Selected;
using gamexModels;
using gamexServices;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class UserViewModel : BaseViewModel, IPasswordViewModel, ISelectedViewModel
{
    private int _id;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }

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

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    public ICommand GetUserCommand { get; }
    public ICommand UpdateUserCommand { get; }
    public ICommand ChangePasswordCommand { get; }
    public ICommand BackToUsersCommand { get; }
    public ICommand GoToAccountViewCommand { get; }
    public ICommand LogoutCommand { get; }

    public UserViewModel(IUserService userService,
                         IAccountStore accountStore,
                         ISingleUser singleUser,
                         IAuthenticationService authenticationService,
                         IAuthenticator authenticator,
                         IRenavigator usersRenavigator,
                         IRenavigator loginRenavigator,
                         IRenavigator accountRenavigator)
    {
        ErrorMessageViewModel = new MessageViewModel();

        GetUserCommand = new GetCommand<UserDto>(this, userService, accountStore, singleUser);
        GetUserCommand.Execute(null);

        UpdateUserCommand = new UpdateUserCommand(this, userService, accountStore, singleUser);
        ChangePasswordCommand = new ChangePasswordCommand(this, authenticationService, accountStore);

        BackToUsersCommand = new RenavigateCommand(usersRenavigator);
        GoToAccountViewCommand = new RenavigateCommand(accountRenavigator);
        LogoutCommand = new LogoutCommand(authenticator, loginRenavigator);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}