using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.State.SelectedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class AddNewUserCommand : ICommand
{
    private readonly IRenavigator _renavigator;
    private readonly ISingleUser _singleUser;

    public AddNewUserCommand(IRenavigator renavigator, ISingleUser singleUser)
    {
        _renavigator = renavigator;
        _singleUser = singleUser;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _singleUser.Id = null;
        _renavigator.Renavigate();
    }
}