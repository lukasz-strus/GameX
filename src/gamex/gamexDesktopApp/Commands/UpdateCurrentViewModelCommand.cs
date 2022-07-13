using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.ViewModels.Factories;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class UpdateCurrentViewModelCommand : ICommand
{
    public event EventHandler CanExecuteChanged;

    private readonly INavigator _navigator;
    private readonly IViewModelFactory _viewModelFactory;

    public UpdateCurrentViewModelCommand(INavigator navigator, IViewModelFactory viewModelFactory)
    {
        _navigator = navigator;
        _viewModelFactory = viewModelFactory;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        if (parameter is ViewType)
        {
            ViewType viewType = (ViewType)parameter;

            _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
        }
    }
}