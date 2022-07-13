using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.SelectedUser;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;

namespace gamexDesktopApp.Commands;

public class GetUserCommand : AsyncCommandBase
{
    private readonly UserViewModel _userViewModel;
    private readonly IUserService _userService;
    private readonly IAccountStore _accountStore;
    private readonly ISingleUser _singleUser;

    public GetUserCommand(UserViewModel userViewModel,
                          IUserService userService,
                          IAccountStore accountStore,
                          ISingleUser singleUsere)
    {
        _userViewModel = userViewModel;
        _userService = userService;
        _accountStore = accountStore;
        _singleUser = singleUsere;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            if (_singleUser != null)
            {
                var game = await _userService.Get(_accountStore.CurrentAccount.Token, (int)_singleUser.Id);
                AssignValues(game);
            }
        }
        catch (Exception)
        {
        }
    }

    private void AssignValues(UserDto userDto)
    {
        _userViewModel.Id = userDto.Id;
        _userViewModel.Login = userDto.Login;
        _userViewModel.Email = userDto.Email;
        _userViewModel.Role = (Role)userDto.RoleId;
    }
}