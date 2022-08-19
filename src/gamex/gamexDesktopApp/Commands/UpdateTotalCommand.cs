using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands;

public class UpdateTotalCommand : AsyncCommandBase
{
    private readonly WalletViewModel _walletViewModel;
    private readonly IUserService _userService;
    private readonly IAccountStore _accountStore;

    public UpdateTotalCommand(WalletViewModel walletViewModel, IUserService userService, IAccountStore accountStore)
    {
        _walletViewModel = walletViewModel;
        _userService = userService;
        _accountStore = accountStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            if (_walletViewModel.TopUp < 0)
            {
                _walletViewModel.ErrorMessage = "An incorrect value was entered";
            }
            else
            {
                var token = _accountStore.CurrentAccount.Token;
                var userId = GetUserIdFromJwt(token);

                var currentUser = await _userService.Get(token, userId);

                var updateUserDto = UpdateTotal(currentUser);

                var response = await _userService.Update(token, userId, updateUserDto);

                if (response == (int)HttpStatusCode.OK)
                {
                    _walletViewModel.ErrorMessage = "Account has been successfully topped up";
                }

                _walletViewModel.RefreshWalletCommand.Execute(null);
            }
        }
        catch (Exception)
        {
            _walletViewModel.ErrorMessage = "Something goes wrong";
        }
    }

    private static int GetUserIdFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var id = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return int.Parse(id);
    }

    private UpdateUserDto UpdateTotal(UserDto user)
    {
        UpdateUserDto updateUserDto = new()
        {
            Total = user.Total
        };
        updateUserDto.Total += _walletViewModel.TopUp;

        return updateUserDto;
    }
}