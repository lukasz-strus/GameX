using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexModelsDto;
using gamexServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands;

public class UpdateTotalCommand : AsyncCommandBase
{
    private WalletViewModel _walletViewModel;
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
                _walletViewModel.ErrorMessage = "Podano błędną wartość";
            }
            else
            {
                var token = _accountStore.CurrentAccount.Token;
                var userId = GetUserIdFromJwt(token);
                var currentUser = await _userService.Get(token, userId);

                UpdateUserDto updateUserDto = new()
                {
                    Total = currentUser.Total
                };
                updateUserDto.Total += _walletViewModel.TopUp;
                await _userService.Update(token, userId, updateUserDto);

                _walletViewModel.GetWalletCommand.Execute(null);

                _walletViewModel.ErrorMessage = "Pomyślnie doładowano konto";
            }
        }
        catch (Exception)
        {
            _walletViewModel.ErrorMessage = "Something goes wrong";
        }
    }

    private int GetUserIdFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var id = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return int.Parse(id);
    }
}