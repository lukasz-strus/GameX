using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.SelectedGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class AddNewGameCommand : ICommand
{
    private readonly IRenavigator _renavigator;
    private readonly ISingleGame _singleGame;

    public AddNewGameCommand(IRenavigator renavigator, ISingleGame singleGame)
    {
        _renavigator = renavigator;
        _singleGame = singleGame;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _singleGame.Id = null;
        _renavigator.Renavigate();
    }
}