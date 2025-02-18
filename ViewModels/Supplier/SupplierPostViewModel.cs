using eshop.api.ViewModels.Address;

namespace eshop.api.ViewModels.Supplier;

public class SupplierPostViewModel
{
  public string Name { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }
  public IList<AddressPostViewModel> Addresses { get; set; }

}
