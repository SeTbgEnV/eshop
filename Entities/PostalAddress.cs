namespace eshop.api.Entities;

public class PostalAddress
{
  public int Id { get; set; }
  public string PostalCode { get; set; }
  public string City { get; set; }
  public IList<Address> Addresses { get; set; }
}
