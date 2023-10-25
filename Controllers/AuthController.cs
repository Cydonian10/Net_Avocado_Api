using System.Security.Claims;
using ApiAvocados.Dto;
using ApiAvocados.Models;
using ApiAvocados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAvocados.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
  public readonly IAuthService _authService;
  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("register")]
  public async Task<ActionResult<UserEntity>> Register([FromBody] UserDto dto)
  {
    var result = await _authService.Register(dto);
    return Ok(new
    {
      message = "Register",
      data = result
    });
  }

  [HttpPost("login")]
  public async Task<ActionResult<string>> Login([FromBody] LoginUserDto dto)
  {

    var result = await _authService.Login(dto);

    return Ok(new
    {
      message = "token",
      data = result
    });
  }

  [HttpGet("profile")]
  [Authorize(Policy = "Customer")]
  public async Task<ActionResult<UserEntity>> Profile()
  {
    var userIdentity = HttpContext.User.Identity as ClaimsIdentity;
    var user = User.Identity!.Name;

    var result = await _authService.Profile(userIdentity!.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)!.Value);

    return Ok(new
    {
      message = "profile",
      data = result
    });
  }
}
