using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.SelectedGame;
using gamexDesktopApp.ViewModels;
using gamexModelsDto;
using gamexServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands;

public class GetGameCommand<T> : AsyncCommandBase
    where T : IGameViewModel
{
    private readonly T _gameViewModel;
    private readonly IGameService _gameService;
    private readonly IAccountStore _accountStore;
    private readonly ISingleGame _singleGame;

    public GetGameCommand(T gameViewModel,
                          IGameService gameService,
                          IAccountStore accountStore,
                          ISingleGame singleGame)
    {
        _gameViewModel = gameViewModel;
        _gameService = gameService;
        _accountStore = accountStore;
        _singleGame = singleGame;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            if (_singleGame != null)
            {
                var game = await _gameService.Get(_accountStore.CurrentAccount.Token, (int)_singleGame.Id);
                AssignValues(game);
            }
        }
        catch (Exception)
        { 
        }
    }

    private void AssignValues(GameDto gameDto)
    {
        _gameViewModel.Id = gameDto.Id;
        _gameViewModel.Name = gameDto.Name;
        _gameViewModel.Description = gameDto.Description;
        _gameViewModel.Price = gameDto.Price;
        _gameViewModel.Total = _accountStore.CurrentAccount.Total;
    }
}