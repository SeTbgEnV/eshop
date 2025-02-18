using Microsoft.AspNetCore.Identity;

namespace eshop.api.Entities;

public class User : IdentityUser
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
}
