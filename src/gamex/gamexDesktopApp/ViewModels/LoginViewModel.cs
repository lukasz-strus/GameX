using gamexDesktopApp.Commands;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _login;

    public string Login
    {
        get
        {
            return _login;
        }
        set
        {
            _login = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanLogin));
        }
    }

    private string _password;

    public string Password
    {
        get
        {
            return _password;
        }
        set
        {
            _password = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanLogin));
        }
    }

    public bool CanLogin => !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    public ICommand LoginCommand { get; }
    public ICommand ViewRegisterCommand { get; }
    public ICommand ViewPasswordReminderCommand { get; }

    public LoginViewModel(IAuthenticator authenticator,
                          IRenavigator loginRenavigator,
                          IRenavigator registerRenavigator,
                          IRenavigator passowrdReminderRenavigator)
    {
        ErrorMessageViewModel = new MessageViewModel();

        LoginCommand = new LoginCommand(this, authenticator, loginRenavigator);
        ViewRegisterCommand = new RenavigateCommand(registerRenavigator);
        ViewPasswordReminderCommand = new RenavigateCommand(passowrdReminderRenavigator);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}