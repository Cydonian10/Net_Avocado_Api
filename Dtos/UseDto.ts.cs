using ApiAvocados.Models;

namespace ApiAvocados.Dto;
public class UserDto
{
  public required string Name { get; set; } = string.Empty;
  public required string Password { get; set; } = string.Empty;
  public required string Email { get; set; } = string.Empty;
  public string? Avatar { get; set; }
  public List<UserRole>? Roles { get; set; }
}

public class LoginUserDto
{
  public required string Password { get; set; } = string.Empty;
  public required string Email { get; set; } = string.Empty;
}