using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.Selected;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using System.Net;

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

            if (IsNewGame()) //add new
            {
                var gameDto = CreateNewGame();

                var response = await _gameService.Create(token, gameDto);

                if (response == (int)HttpStatusCode.OK)
                {
                    _gameAdminViewModel.ErrorMessage = "The game has been added";
                }
            }
            else //update existing
            {
                var game = await _gameService.Get(token, gameId);

                UpdateGameDto updateGameDto = UpdateGame(game);

                var response = await _gameService.Update(token, gameId, updateGameDto);

                if (response == (int)HttpStatusCode.OK)
                {
                    _gameAdminViewModel.ErrorMessage = "The game has been updated";
                }
            }

            _gameAdminViewModel.GamesViewCommand.Execute(null);
        }
        catch (Exception)
        {
            _gameAdminViewModel.ErrorMessage = "Incorrect data";
        }
    }

    private bool IsNewGame() => _singleGame.Id == null;

    private CreateGameDto CreateNewGame() => new()
    {
        Name = _gameAdminViewModel.Name,
        Description = _gameAdminViewModel.Description,
        Price = _gameAdminViewModel.Price
    };

    private UpdateGameDto UpdateGame(GameDto game)
    {
        UpdateGameDto updateGameDto = new()
        {
            Description = _gameAdminViewModel.Description,
            Price = _gameAdminViewModel.Price
        };

        if (_gameAdminViewModel.Name != game.Name)
            updateGameDto.Name = _gameAdminViewModel.Name;

        return updateGameDto;
    }
}