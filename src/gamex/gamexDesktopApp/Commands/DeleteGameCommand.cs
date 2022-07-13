using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.SelectedGame;
using gamexDesktopApp.ViewModels;
using gamexServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gamexDesktopApp.Commands;

public class DeleteGameCommand<T> : AsyncCommandBase
    where T : IPagesViewModel
{
    private readonly T _gameViewModel;
    private readonly ISingleGame _singleGame;
    private readonly IGameService _gameService;
    private readonly IAccountStore _accountStore;

    public DeleteGameCommand(T gameViewModel,
                             ISingleGame singleGame,
                             IGameService gameService,
                             IAccountStore accountStore)
    {
        _gameViewModel = gameViewModel;
        _singleGame = singleGame;
        _gameService = gameService;
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
            int id = (int)_singleGame.Id;
            await _gameService.Delete(token, id);
            _gameViewModel.ViewListCommand.Execute(null);
        }
        catch (Exception)
        {
            _gameViewModel.ErrorMessage = "Coś poszło nie tak";
        }
    }
}