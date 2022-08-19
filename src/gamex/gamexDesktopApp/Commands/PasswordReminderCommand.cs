using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.ViewModels;

namespace gamexDesktopApp.Commands;

public class PasswordReminderCommand : AsyncCommandBase
{
    private readonly PasswordReminderViewModel _passwordReminderViewModel;
    private readonly IAuthenticator _authenticator;

    public PasswordReminderCommand(PasswordReminderViewModel passwordReminderViewModel,
                                   IAuthenticator authenticator)
    {
        _passwordReminderViewModel = passwordReminderViewModel;
        _authenticator = authenticator;
    }

    public override Task ExecuteAsync(object parameter)
    {
        //TODO implement Password Reminder
        throw new NotImplementedException();
    }
}