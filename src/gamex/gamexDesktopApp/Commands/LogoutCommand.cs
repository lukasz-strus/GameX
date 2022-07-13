using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class LogoutCommand : ICommand
{
    private readonly IAuthenticator _authenticator;
    private readonly IRenavigator _renavigator;

    public LogoutCommand(IAuthenticator authenticator, IRenavigator renavigator)
    {
        _authenticator = authenticator;
        _renavigator = renavigator;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _authenticator.Logout();
        _renavigator.Renavigate();
    }
}