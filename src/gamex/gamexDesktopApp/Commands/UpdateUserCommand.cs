using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.SelectedUser;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands;

public class UpdateUserCommand : AsyncCommandBase
{
    private readonly UserViewModel _userViewModel;
    private readonly IUserService _userService;
    private readonly IAccountStore _accountStore;
    private readonly ISingleUser _singleUser;

    public UpdateUserCommand(UserViewModel userViewModel,
                             IUserService userService,
                             IAccountStore accountStore,
                             ISingleUser singleUser)
    {
        _userViewModel = userViewModel;
        _userService = userService;
        _accountStore = accountStore;
        _singleUser = singleUser;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            var token = _accountStore.CurrentAccount.Token;
            var userId = _userViewModel.Id;

            var game = await _userService.Get(token, userId);
            UpdateUserDto updateUserDto = new()
            {
                RoleId = (int)_userViewModel.Role
            };

            if (_userViewModel.Login != game.Login)
                updateUserDto.Login = _userViewModel.Login;
            if (_userViewModel.Email != game.Email)
                updateUserDto.Email = _userViewModel.Email;

            var response = await _userService.Update(token, userId, updateUserDto);

            if (response == 200)
                _userViewModel.ErrorMessage = "Użytkownik został zaktualizowany";

            _userViewModel.BackToUsersCommand.Execute(null);
        }
        catch (Exception)
        {
            _userViewModel.ErrorMessage = "Błędne dane. Sprawdź czy login użytkownika istnieje już w bazie.";
        }
    }

    private bool IsNewUser() => _singleUser.Id == null;
}