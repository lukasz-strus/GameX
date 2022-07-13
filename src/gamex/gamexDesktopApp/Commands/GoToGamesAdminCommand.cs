using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Navigators;
using gamexDesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace gamexDesktopApp.Commands;

public class GoToGamesAdminCommand : ICommand
{
    private readonly IAccountStore _accountStore;
    private readonly IRenavigator _adminRenavigator;
    private readonly IRenavigator _salesRenavigator;

    public GoToGamesAdminCommand(IAccountStore accountStore, IRenavigator adminRenavigator, IRenavigator salesRenavigator)
    {
        _adminRenavigator = adminRenavigator;
        _salesRenavigator = salesRenavigator;
        _accountStore = accountStore;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        var accountRole = _accountStore.CurrentAccount.Role;

        return accountRole == Role.Admin || accountRole == Role.Seller;
    }

    public void Execute(object parameter)
    {
        switch (_accountStore.CurrentAccount.Role)
        {
            case Role.Admin:
                _adminRenavigator.Renavigate();
                break;

            case Role.Seller:
                _salesRenavigator.Renavigate();
                break;
        }
    }
}