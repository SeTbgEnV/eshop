using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eshop.api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace eshop.api.Services;

public class TokenService
{
  private readonly UserManager<User> _userManager;
  private readonly IConfiguration _config;
  public TokenService(UserManager<User> userManager, IConfiguration config)
  {
    _config = config;
    _userManager = userManager;
  }

  // Metod för att skapa ett Json Web Token(JWT) och returnera en sträng som representerar
  // biljetten(token).
  public async Task<string> CreateToken(User user)
  {
    // Skapar vår payload...
    /**************************************************************/
    var claims = new List<Claim>{
      new(ClaimTypes.Email, user.Email),
      new(ClaimTypes.Name, user.UserName),
      new("FirstName",user.FirstName),
      new("LastName", user.LastName),
    };

    // Tar reda vilka roller som användaren tillhör...
    var roles = await _userManager.GetRolesAsync(user);

    // Loopa igenom rollerna och placera varje roll som ett påstående(claim)
    foreach (var role in roles)
    {
      claims.Add(new(ClaimTypes.Role, role));
      // claims.Add(new Claim(ClaimTypes.Role, role));
    }
    /**************************************************************/

    // Skapa eller generera en signature...
    /**************************************************************/
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["tokenSettings:tokenKey"]));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
    /**************************************************************/

    // Skapa vårt JWT token(biljett)...
    /**************************************************************/
    var options = new JwtSecurityToken(
      issuer: null,
      audience: null,
      claims: claims,
      expires: DateTime.Now.AddDays(5),
      signingCredentials: credentials
    );
    /**************************************************************/

    return new JwtSecurityTokenHandler().WriteToken(options);
  }
}
