using AutoMapper;
using gamexDesktopApp.Helpers;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexModels;
using gamexServices;
using System.ComponentModel;
using System.IO;

namespace gamexDesktopApp.Commands;

public class GetGamesListCommand<T> : AsyncCommandBase
        where T : IGamesViewModel
{
    private readonly T _gamesViewModel;
    private readonly IGameService _gameService;
    private GetAllResult<GameDto> _getAllResult;
    private readonly IAccountStore _accountStore;
    private readonly IFileService _fileService;

    public GetGamesListCommand(T gamesViewModel,
                               IGameService gameService,
                               IAccountStore accountStore,
                               IFileService fileService)
    {
        _gamesViewModel = gamesViewModel;
        _gameService = gameService;
        _accountStore = accountStore;
        _fileService = fileService;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            _gamesViewModel.Games.GamesCollection.Clear();

            var token = _accountStore.CurrentAccount.Token;
            _getAllResult = await GetAllResult(token);
            AssignValues(token, _getAllResult);
        }
        catch (Exception)
        {
            _gamesViewModel.ErrorMessage = "Failed";
        }
    }

    private Task<GetAllResult<GameDto>> GetAllResult(string token) =>
         _gameService.GetAll(token, Query());

    private GetAllQuery Query() =>
        new()
        {
            SearchPhrase = _gamesViewModel.SearchPhrase,
            PageNumber = _gamesViewModel.PageNumber,
            PageSize = _gamesViewModel.PageSize,
            SortBy = _gamesViewModel.SortBy.ToString(),
            SortDirection = _gamesViewModel.SortDirection
        };

    private void AssignValues(string token, GetAllResult<GameDto> getAllResult)
    {
        AddToGamesCollection(token, getAllResult.Items);
        _gamesViewModel.TotalPages = getAllResult.TotalPages;
        _gamesViewModel.ItemsFrom = getAllResult.ItemsFrom;
        _gamesViewModel.ItemsTo = getAllResult.ItemsTo;
        _gamesViewModel.TotalItemsCount = getAllResult.TotalItemsCount;
        _gamesViewModel.Total = _accountStore.CurrentAccount.Total;
    }

    private async void AddToGamesCollection(string token, List<GameDto> dto)
    {
        foreach (var item in dto)
        {
            var imageStream = await GetGamesImages(token, item.Id);

            _gamesViewModel.Games.GamesCollection.Add(MapFromGameDto(item, imageStream));
        }
    }

    private Game MapFromGameDto(GameDto dto, byte[] imageStream) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Source = FileHelper.LoadImage(imageStream)
        };

    private async Task<byte[]> GetGamesImages(string token, int gameId)
    {
        var imageStream = await _fileService.GetGameImage(token, gameId);

        return imageStream;
    }
}