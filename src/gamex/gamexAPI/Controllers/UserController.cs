using gamexAPI.Entities;
using gamexAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gamexModels;
using gamexAPI.Models;

namespace gamexAPI.Controllers;

[Route("api/user")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public ActionResult<IEnumerable<User>> GetAllUsers([FromQuery] GetAllQuery query)
    {
        var users = _userService.GetAll(query);

        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Seller,User")]
    public ActionResult<User> Get([FromRoute] int id)
    {
        var user = _userService.Get(id);

        return Ok(user);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult Delete([FromRoute] int id)
    {
        _userService.Delete(id);

        return NoContent();
    }

    [HttpPut("{id}/password")]
    [Authorize(Roles = "Admin,Seller,User")]
    public ActionResult ChangePassword([FromRoute] int id, [FromBody] UserPasswordDto dto)
    {
        _userService.ChangePassword(id, dto);

        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Seller,User")]
    public ActionResult Update([FromRoute] int id, [FromBody] UpdateUserDto dto)
    {
        _userService.Update(id, dto);

        return Ok();
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult Register([FromBody] RegisterUserDto dto)
    {
        _userService.RegisterUser(dto);

        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult Login([FromBody] LoginDto dto)
    {
        string token = _userService.GenerateJwt(dto);

        return Ok(token);
    }
}