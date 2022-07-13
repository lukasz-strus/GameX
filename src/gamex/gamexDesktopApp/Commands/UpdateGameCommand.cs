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

public class UpdateGameCommand : AsyncCommandBase
{
    private readonly GameAdminViewModel _gameAdminViewModel;
    private readonly IGameService _gameService;
    private readonly IAccountStore _accountStore;
    private readonly ISingleGame _singleGame;

    public UpdateGameCommand(GameAdminViewModel gameAdminViewModel,
                             IGameService gameService,
                             IAccountStore accountStore,
                             ISingleGame singleGame)
    {
        _gameAdminViewModel = gameAdminViewModel;
        _gameService = gameService;
        _accountStore = accountStore;
        _singleGame = singleGame;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            var token = _accountStore.CurrentAccount.Token;
            var gameId = _gameAdminViewModel.Id;

            if (IsNewGame())
            {
                CreateGameDto gameDto = new()
                {
                    Name = _gameAdminViewModel.Name,
                    Description = _gameAdminViewModel.Description,
                    Price = _gameAdminViewModel.Price
                };

                var response = await _gameService.Create(token, gameDto);
                if (response == 200)
                    _gameAdminViewModel.ErrorMessage = "Gra została dodana";
            }
            else
            {
                var game = await _gameService.Get(token, gameId);
                UpdateGameDto updateGameDto = new()
                {
                    Description = _gameAdminViewModel.Description,
                    Price = _gameAdminViewModel.Price
                };

                if (_gameAdminViewModel.Name != game.Name)
                    updateGameDto.Name = _gameAdminViewModel.Name;

                var response = await _gameService.Update(token, gameId, updateGameDto);

                if (response == 200)
                    _gameAdminViewModel.ErrorMessage = "Gra została zaktualizowana";
            }

            _gameAdminViewModel.BackToGamesCommand.Execute(null);
        }
        catch (Exception)
        {
            _gameAdminViewModel.ErrorMessage = "Błędne dane. Sprawdź czy nazwa gry istnieje już w bazie.";
        }
    }

    private bool IsNewGame() => _singleGame.Id == null;
}