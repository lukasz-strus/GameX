using gamexDesktopApp.Commands;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class PasswordReminderViewModel : BaseViewModel
{
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

    public MessageViewModel ErrorMessageViewModel { get; }

    public string ErrorMessage
    {
        set => ErrorMessageViewModel.Message = value;
    }

    public ICommand PasswordRemindCommand { get; }
    public ICommand ViewRegisterCommand { get; }

    public PasswordReminderViewModel(IAuthenticator authenticator,
                                     IRenavigator registerRenavigator)
    {
        ErrorMessageViewModel = new MessageViewModel();
        ViewRegisterCommand = new RenavigateCommand(registerRenavigator);
        PasswordRemindCommand = new PasswordReminderCommand(this, authenticator);
    }

    public override void Dispose()
    {
        ErrorMessageViewModel.Dispose();

        base.Dispose();
    }
}