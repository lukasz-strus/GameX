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
    private static bool isImagesLoaded = false;

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
            Mapping(token, _getAllResult);
        }
        catch (Exception)
        {
            _gamesViewModel.ErrorMessage = "Failed";
        }
    }

    private async Task<GetAllResult<GameDto>> GetAllResult(string token) =>
        await _gameService.GetAll(token, GetAllQuery());

    private GetAllQuery GetAllQuery() =>
        new()
        {
            SearchPhrase = _gamesViewModel.SearchPhrase,
            PageNumber = _gamesViewModel.PageNumber,
            PageSize = _gamesViewModel.PageSize,
            SortBy = _gamesViewModel.SortBy.ToString(),
            SortDirection = _gamesViewModel.SortDirection
        };

    private void Mapping(string token, GetAllResult<GameDto> getAllResult)
    {
        AddToGamesCollection(token, getAllResult.Items);
        _gamesViewModel.TotalPages = getAllResult.TotalPages;
        _gamesViewModel.ItemsFrom = getAllResult.ItemsFrom;
        _gamesViewModel.ItemsTo = getAllResult.ItemsTo;
        _gamesViewModel.TotalItemsCount = getAllResult.TotalItemsCount;
        _gamesViewModel.Total = _accountStore.CurrentAccount.Total;
    }

    private void AddToGamesCollection(string token, List<GameDto> dto)
    {
        foreach (var item in dto)
        {
            _gamesViewModel.Games.GamesCollection.Add(MapFromGameDto(item));

            if (!isImagesLoaded)
                GetGamesImages(token, item.Id);
        }

        isImagesLoaded = true;
    }

    private Game MapFromGameDto(GameDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Source = SourceHelper.SetSource(dto.Id)
        };

    private async void GetGamesImages(string token, int gameId)
    {
        var fullPath = string.Concat(SourceHelper.GetProjectDirectory(), $"/Images/Games/");

        var myFile = File.Create(SourceHelper.GetFilePath(fullPath, gameId.ToString()));
        myFile.Close();

        Thread.Sleep(100);

        await _fileService.GetGameImage(token, gameId, fullPath);
    }
}