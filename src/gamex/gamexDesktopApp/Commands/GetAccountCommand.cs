using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace gamexDesktopApp.Commands;

public class GetAccountCommand : AsyncCommandBase
{
    private readonly AccountViewModel _accountViewModel;
    private readonly IUserService _userService;
    private readonly IAccountStore _accountStore;

    public GetAccountCommand(AccountViewModel accountViewModel, IUserService userService, IAccountStore accountStore)
    {
        _accountViewModel = accountViewModel;
        _userService = userService;
        _accountStore = accountStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            var token = _accountStore.CurrentAccount.Token;
            var userId = GetUserIdFromJwt(token);
            var currentUser = await _userService.Get(token, userId);
            AssignValues(currentUser);
        }
        catch (Exception)
        {
            _accountViewModel.ErrorMessage = "Something goes wrong.";
        }
    }

    private int GetUserIdFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var id = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return int.Parse(id);
    }

    private void AssignValues(UserDto userDto)
    {
        _accountStore.CurrentAccount.Login = userDto.Login;
        _accountStore.CurrentAccount.Email = userDto.Email;
        _accountStore.CurrentAccount.Total = userDto.Total;
        _accountStore.CurrentAccount.Role = (Role)userDto.RoleId;
        AssignValuesToViewModel();
    }

    private void AssignValuesToViewModel()
    {
        _accountViewModel.Login = _accountStore.CurrentAccount.Login;
        _accountViewModel.Email = _accountStore.CurrentAccount.Email;
        _accountViewModel.Role = _accountStore.CurrentAccount.Role;
        _accountViewModel.Total = _accountStore.CurrentAccount.Total;
    }
}