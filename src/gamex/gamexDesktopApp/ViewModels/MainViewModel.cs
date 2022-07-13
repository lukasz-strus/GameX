using gamexDesktopApp.Commands;
using gamexDesktopApp.State.Authenticators;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.ViewModels.Factories;
using System.Windows.Input;

namespace gamexDesktopApp.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IViewModelFactory _viewModelFactory;
    private readonly INavigator _navigator;
    private readonly IAuthenticator _authenticator;

    public bool IsLoggedIn => _authenticator.IsLoggedIn;

    public BaseViewModel CurrentViewModel => _navigator.CurrentViewModel;

    public ICommand UpdateCurrentViewModelCommand { get; }

    public MainViewModel(
        INavigator navigator,
        IViewModelFactory viewModelFactory,
        IAuthenticator authenticator)
    {
        _navigator = navigator;
        _viewModelFactory = viewModelFactory;
        _authenticator = authenticator;

        _navigator.StateChanged += Navigator_StateChanged;
        _authenticator.StateChanged += Authenticator_StateChanged;

        UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
        UpdateCurrentViewModelCommand.Execute(ViewType.Login);
    }

    private void Authenticator_StateChanged()
    {
        OnPropertyChanged(nameof(IsLoggedIn));
    }

    private void Navigator_StateChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public override void Dispose()
    {
        _navigator.StateChanged -= Navigator_StateChanged;
        _authenticator.StateChanged -= Authenticator_StateChanged;

        base.Dispose();
    }
}