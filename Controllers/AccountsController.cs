using eshop.api.Entities;
using eshop.api.Services;
using eshop.api.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
  private readonly UserManager<User> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly TokenService _tokenService;
  public AccountsController(UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    TokenService tokenService)
  {
    _roleManager = roleManager;
    _tokenService = tokenService;
    _userManager = userManager;
  }

  [HttpPost("register")]
  public async Task<ActionResult> RegisterUser(RegisterUserViewModel model)
  {
    try
    {
      var user = await AddUser(model);
      await _userManager.AddToRoleAsync(user, "User");
      return StatusCode(201);
    }
    catch (Exception ex)
    {
      return BadRequest(new { success = false, message = ex.Message });
    }
  }

  [HttpPost("registerwithrole")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> RegisterUserWithRole(RegisterUserWithRoleViewModel model)
  {
    try
    {
      var role = await _roleManager.FindByNameAsync(model.RoleName);

      if (role is null)
      {
        return BadRequest(new { success = false, message = $"Rollen {model.RoleName} finns inte" });
      }

      var user = await AddUser(model);
      await _userManager.AddToRoleAsync(user, model.RoleName);
      return StatusCode(201);
    }
    catch (Exception ex)
    {
      return BadRequest(new { success = false, message = ex.Message });
    }
  }

  [HttpPost("login")]
  public async Task<ActionResult> LoginUser(LoginViewModel model)
  {
    // Steg 1. Hämta användaren om hen finns...
    var user = await _userManager.FindByNameAsync(model.UserName);

    // Steg 2. Kontrollera om användare fanns
    if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
    {
      return Unauthorized(new { success = false, message = "Unauthorized" });
    }

    return Ok(new { success = true, email = user.Email, token = await _tokenService.CreateToken(user) });
  }

  [HttpPost("changepassword")]
  public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
  {
    var user = await _userManager.FindByNameAsync(model.UserName);

    if (user is null)
    {
      return BadRequest(new { success = false });
    }
    await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
    return StatusCode(201);
  }
  private async Task<User> AddUser(RegisterUserViewModel model)
  {
    model.UserName = model.Email;
    var user = new User
    {
      UserName = model.UserName,
      Email = model.Email,
      FirstName = model.FirstName,
      LastName = model.LastName,
    };

    var result = await _userManager.CreateAsync(user, model.Password);

    if (result.Succeeded)
    {
      // return await _userManager.FindByNameAsync(model.UserName);
      return user;
    }

    throw new Exception($"Det gick inte att lägga till användare, {result.Errors}");
  }
}
