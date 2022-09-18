using gamexDesktopApp.Exceptions;
using gamexDesktopApp.HostBuilders;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands;

public class LoginCommand : AsyncCommandBase
{
    private readonly LoginViewModel _loginViewModel;
    private readonly IAuthenticator _authenticator;
    private readonly IRenavigator _renavigator;

    public LoginCommand(LoginViewModel loginViewModel, IAuthenticator authenticator, IRenavigator renavigator)
    {
        _loginViewModel = loginViewModel;
        _authenticator = authenticator;
        _renavigator = renavigator;

        _loginViewModel.PropertyChanged += LoginViewModel_PropertyChanged;
    }

    public override bool CanExecute(object parameter)
    {
        return _loginViewModel.CanLogin && base.CanExecute(parameter);
    }

    public override async Task ExecuteAsync(object parameter)
    {
        _loginViewModel.ErrorMessage = string.Empty;

        try
        {
            await _authenticator.Login(_loginViewModel.Login, _loginViewModel.Password);

            _renavigator.Renavigate();
        }
        catch (UserNotFoundException)
        {
            _loginViewModel.ErrorMessage = "Login does not exist.";
        }
        catch (InvalidPasswordException)
        {
            _loginViewModel.ErrorMessage = "Incorrect password.";
        }
        catch (Exception)
        {
            _loginViewModel.ErrorMessage = "Login failed.";
        }
    }

    private void LoginViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(LoginViewModel.CanLogin))
        {
            OnCanExecuteChanged();
        }
    }
}