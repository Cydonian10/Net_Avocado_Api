using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiAvocados.Dto;
using ApiAvocados.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiAvocados.Services;
public class AuthService : IAuthService
{
  public readonly ApiContext _dbContext;
  private readonly IConfiguration _configuration;

  public AuthService(ApiContext dbContext, IConfiguration configuration)
  {
    _dbContext = dbContext;
    _configuration = configuration;
  }


  public async Task<string?> Login(LoginUserDto dto)
  {
    var user = await _dbContext.Users.FirstOrDefaultAsync((u) => u.Email == dto.Email);

    if (user == null)
    {
      return null;
    }

    if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
    {
      return null;
    }

    string token = CreateToken(user);

    return token;
  }

  public async Task<UserEntity> Register(UserDto dto)
  {
    string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

    var userId = Guid.NewGuid();

    var newUser = new UserEntity
    {
      Id = userId,
      Avatar = dto.Avatar,
      Email = dto.Email,
      PasswordHash = passwordHash,
      Roles = dto.Roles,
      Name = dto.Name,
    };

    await _dbContext.Users.AddAsync(newUser);
    await _dbContext.SaveChangesAsync();

    return newUser;
  }

  public string CreateToken(UserEntity user)
  {

    List<Claim> claims = new()
    {
      new (ClaimTypes.NameIdentifier,user.Id.ToString()),
      new (ClaimTypes.Email, user.Email),
      new (ClaimTypes.Name, user.Name),
    };

    foreach (var item in user.Roles!)
    {
      claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
    }

    var key = new SymmetricSecurityKey(
     Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!)
   );

    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: cred);

    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return jwt;
  }

  public async Task<UserEntity?> Profile(string email)
  {
    var user = await _dbContext.Users.FirstOrDefaultAsync((u) => u.Email == email);

    if (user == null)
    {
      return null;
    }

    return user!;
  }

}

public interface IAuthService
{
  Task<UserEntity> Register(UserDto dto);
  Task<string?> Login(LoginUserDto dto);
  string CreateToken(UserEntity user);
  Task<UserEntity?> Profile(string email);
}
