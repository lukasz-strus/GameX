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

public class BuyGameCommand : AsyncCommandBase
{
    private readonly GameViewModel _gameViewModel;
    private readonly IGameService _gameService;
    private readonly IAccountStore _accountStore;
    private readonly IUserService _userService;

    public BuyGameCommand(GameViewModel gameViewModel, IGameService gameService, IAccountStore accountStore, IUserService userService)
    {
        _gameViewModel = gameViewModel;
        _gameService = gameService;
        _accountStore = accountStore;
        _userService = userService;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            var serial = await _gameService.GetSerialKey(_accountStore.CurrentAccount.Token, _gameViewModel.Id);

            _gameViewModel.SerialKey = serial;

            var token = _accountStore.CurrentAccount.Token;
            var userId = GetUserIdFromJwt(token);
            var user = await _userService.Get(token, userId);

            _gameViewModel.Total = user.Total;
        }
        catch (Exception)
        {
            _gameViewModel.ErrorMessage = "Nie masz środków na koncie";
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