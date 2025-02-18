namespace eshop.api.Entities;

public class Supplier
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }
  public SupplierProduct SupplierProducts { get; set; }
  public IList<SupplierAddress> SupplierAddresses { get; set; } = [];
}
