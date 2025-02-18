using System.ComponentModel.DataAnnotations;

namespace eshop.api.Entities;

public class Customer
{
  public int Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }

  public IList<CustomerAddress> CustomerAddresses { get; set; }
}
