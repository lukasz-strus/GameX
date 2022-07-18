﻿using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.State.SelectedGame;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using gamexDesktopApp.Helpers;
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
    private readonly IFileService _fileService;

    public GetGameCommand(T gameViewModel,
                          IGameService gameService,
                          IAccountStore accountStore,
                          ISingleGame singleGame,
                          IFileService fileService)
    {
        _gameViewModel = gameViewModel;
        _gameService = gameService;
        _accountStore = accountStore;
        _singleGame = singleGame;
        _fileService = fileService;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            if (_singleGame != null)
            {
                var token = _accountStore.CurrentAccount.Token;
                var gameId = (int)_singleGame.Id;
                var game = await _gameService.Get(token, gameId);
                AssignValues(game);

                var fullPath = string.Concat(SourceHelper.GetProjectDirectory(), $"/Images/Games/");
                await _fileService.GetGameImage(token, gameId, fullPath); //TODO Wyjątek 404
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