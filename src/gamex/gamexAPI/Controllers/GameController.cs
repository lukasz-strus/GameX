using gamexAPI.Entities;
using gamexAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gamexModels;
using gamexAPI.Models;

namespace gamexAPI.Controllers;

[Route("api/game")]
[ApiController]
[Authorize]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Seller")]
    public ActionResult Create([FromBody] CreateGameDto dto)
    {
        var id = _gameService.Create(dto);

        return Created($"/api/game/{id}", null);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Game>> GetAllGames([FromQuery] GetAllQuery query)
    {
        var games = _gameService.GetAll(query);

        return Ok(games);
    }

    [HttpGet("{id}")]
    public ActionResult<Game> Get([FromRoute] int id)
    {
        var game = _gameService.Get(id);

        return Ok(game);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    public ActionResult Delete([FromRoute] int id)
    {
        _gameService.Delete(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Seller")]
    public ActionResult Update([FromRoute] int id, [FromBody] UpdateGameDto dto)
    {
        _gameService.Update(id, dto);

        return Ok();
    }

    [HttpGet("{userId}/{gameId}")]
    public ActionResult GetSerialKey([FromRoute] int userId, [FromRoute] int gameId)
    {
        var key = _gameService.GetSerialKey(userId, gameId);

        return Ok(key);
    }
}