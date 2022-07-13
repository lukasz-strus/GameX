using gamexDesktopApp.ViewModels;

namespace gamexDesktopApp.State.Navigators;

public enum ViewType
{
    Account,
    Games,
    Game,
    Login,
    PasswordReminder,
    Registration,
    User,
    Users
}

public interface INavigator
{
    BaseViewModel CurrentViewModel { get; set; }

    event Action StateChanged;
}

public class Navigator : INavigator
{
    private BaseViewModel _currentViewModel;

    public BaseViewModel CurrentViewModel
    {
        get
        {
            return _currentViewModel;
        }
        set
        {
            _currentViewModel?.Dispose();

            _currentViewModel = value;
            StateChanged?.Invoke();
        }
    }

    public event Action StateChanged;
}