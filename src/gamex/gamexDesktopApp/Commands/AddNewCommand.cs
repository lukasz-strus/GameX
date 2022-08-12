using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.Selected;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class AddNewCommand : ICommand
{
    private readonly IRenavigator _renavigator;
    private readonly ISelected _selected;

    public AddNewCommand(IRenavigator renavigator, ISelected selected)
    {
        _renavigator = renavigator;
        _selected = selected;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _selected.Id = null;
        _renavigator.Renavigate();
    }
}