using gamexDesktopApp.Commands;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class RegistrationViewModel : BaseViewModel
{
    private string _login;

    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanRegister));
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
            OnPropertyChanged(nameof(CanRegister));
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
            OnPropertyChanged(nameof(CanRegister));
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
            OnPropertyChanged(nameof(CanRegister));
        }
    }

    private string _confirmEmail;

    public string ConfirmEmail
    {
        get => _confirmEmail;
        set
        {
            _confirmEmail = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CanRegister));
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
            OnPropertyChanged(nameof(CanRegister));
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
            OnPropertyChanged(nameof(CanRegister));
        }
    }

    public bool CanRegister =>
        !string.IsNullOrEmpty(Login) &&
        !string.IsNullOrEmpty(Password) &&
        !string.IsNullOrEmpty(ConfirmPassword) &&
        !string.IsNullOrEmpty(Email) &&
        !string.IsNullOrEmpty(ConfirmEmail);

    public ICommand RegisterCommand { get; }

    public ICommand ViewLoginCommand { get; }

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    public RegistrationViewModel(IAuthenticator authenticator, IRenavigator registerRenavigator, IRenavigator loginRenavigator)
    {
        ErrorMessageViewModel = new MessageViewModel();

        RegisterCommand = new RegisterCommand(this, authenticator, registerRenavigator);
        ViewLoginCommand = new RenavigateCommand(loginRenavigator);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}