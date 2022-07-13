using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.SelectedUser;
using gamexDesktopApp.ViewModels;
using gamexServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands;

public class DeleteUserCommand : AsyncCommandBase
{
    private readonly UsersViewModel _usersViewModel;
    private readonly ISingleUser _singleUser;
    private readonly IUserService _userService;
    private readonly IAccountStore _accountStore;

    public DeleteUserCommand(UsersViewModel usersViewModel,
                             ISingleUser singleUser,
                             IUserService userService,
                             IAccountStore accountStore)
    {
        _usersViewModel = usersViewModel;
        _singleUser = singleUser;
        _userService = userService;
        _accountStore = accountStore;
    }

    public override bool CanExecute(object parameter)
    {
        return true;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            var token = _accountStore.CurrentAccount.Token;
            int id = (int)_singleUser.Id;
            await _userService.Delete(token, id);
            _usersViewModel.ViewListCommand.Execute(null);
        }
        catch (Exception)
        {
            _usersViewModel.ErrorMessage = "Coś poszło nie tak";
        }
    }
}