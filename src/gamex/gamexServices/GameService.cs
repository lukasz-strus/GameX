using Flurl.Http;
using gamexModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace gamexServices;

public interface IGameService
{
    Task<int> Create(string token, CreateGameDto createGameDto);

    Task<int> Delete(string token, int id);

    Task<GameDto> Get(string token, int id);

    Task<GetAllResult<GameDto>> GetAll(string token, GetAllQuery query);

    Task<string> GetSerialKey(string token, int gameId);

    Task<int> Update(string token, int gameId, UpdateGameDto updateGameDto);
}

public class GameService : IGameService
{
    private readonly string _baseUrl =
        "https://gamex-api-app.azurewebsites.net/api/game";

    public async Task<int> Create(string token, CreateGameDto createGameDto)
    {
        var response = await _baseUrl
            .WithOAuthBearerToken(token)
            .PostJsonAsync(createGameDto);

        return response.StatusCode;
    }

    public async Task<GetAllResult<GameDto>> GetAll(string token, GetAllQuery query)
    {
        var response = await _baseUrl
            .WithOAuthBearerToken(token)
            .SetQueryParams(new
            {
                searchPhrase = query.SearchPhrase,
                pageSize = query.PageSize,
                pageNumber = query.PageNumber,
                sortyBy = query.SortBy,
                sortDirection = query.SortDirection
            })
            .GetAsync()
            .ReceiveJson<GetAllResult<GameDto>>();

        return response;
    }

    public async Task<GameDto> Get(string token, int id)
    {
        var response = await (_baseUrl + $"/{id}")
            .WithOAuthBearerToken(token)
            .GetAsync()
            .ReceiveJson<GameDto>();

        return response;
    }

    public async Task<int> Delete(string token, int id)
    {
        var response = await (_baseUrl + $"/{id}")
            .WithOAuthBearerToken(token)
            .DeleteAsync();

        return response.StatusCode;
    }

    public async Task<int> Update(string token, int gameId, UpdateGameDto updateGameDto)
    {
        var response = await (_baseUrl + $"/{gameId}")
            .WithOAuthBearerToken(token)
            .PutJsonAsync(updateGameDto);

        return response.StatusCode;
    }

    public async Task<string> GetSerialKey(string token, int gameId)
    {
        var userId = GetUserIdFromJwt(token);

        var response = await (_baseUrl + $"/{userId}/{gameId}")
            .WithOAuthBearerToken(token)
            .GetAsync()
            .ReceiveJson<GameSerialDto>();

        return response.Value;
    }

    private int GetUserIdFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);

        var id = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return int.Parse(id);
    }
}