using AutoMapper;
using gamexDesktopApp.Models;
using gamexDesktopApp.State.Accounts;
using gamexDesktopApp.ViewModels;
using gamexModelsDto;
using gamexServices;
using System.ComponentModel;

namespace gamexDesktopApp.Commands;

public class GetGamesListCommand<T> : AsyncCommandBase
        where T : IGamesViewModel
{
    private readonly T _gamesViewModel;
    private readonly IGameService _gameService;
    private GetAllResult<GameDto> _getAllResult;
    private readonly IAccountStore _accountStore;

    public GetGamesListCommand(T gamesViewModel, IGameService gameService, IAccountStore accountStore)
    {
        _gamesViewModel = gamesViewModel;
        _gameService = gameService;
        _accountStore = accountStore;
    }

    public override async Task ExecuteAsync(object parameter)
    {
        try
        {
            _gamesViewModel.Games.GamesCollection.Clear();
            _getAllResult = await GetAllResult();
            Mapping(_getAllResult);
        }
        catch (Exception)
        {
            _gamesViewModel.ErrorMessage = "Failed";
        }
    }

    private async Task<GetAllResult<GameDto>> GetAllResult() =>
        await _gameService.GetAll(_accountStore.CurrentAccount.Token, GetAllQuery());

    private GetAllQuery GetAllQuery() =>
        new()
        {
            SearchPhrase = _gamesViewModel.SearchPhrase,
            PageNumber = _gamesViewModel.PageNumber,
            PageSize = _gamesViewModel.PageSize,
            SortBy = _gamesViewModel.SortBy.ToString(),
            SortDirection = _gamesViewModel.SortDirection
        };

    private void Mapping(GetAllResult<GameDto> getAllResult)
    {
        AddToGamesCollection(getAllResult.Items);
        _gamesViewModel.TotalPages = getAllResult.TotalPages;
        _gamesViewModel.ItemsFrom = getAllResult.ItemsFrom;
        _gamesViewModel.ItemsTo = getAllResult.ItemsTo;
        _gamesViewModel.TotalItemsCount = getAllResult.TotalItemsCount;
        _gamesViewModel.Total = _accountStore.CurrentAccount.Total;
    }

    private void AddToGamesCollection(List<GameDto> dto)
    {
        foreach (var item in dto)
            _gamesViewModel.Games.GamesCollection.Add(MapFromGameDto(item));
    }

    private Game MapFromGameDto(GameDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price
        };
}