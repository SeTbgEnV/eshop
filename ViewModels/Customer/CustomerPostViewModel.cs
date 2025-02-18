using eshop.api.ViewModels.Address;

namespace eshop.api;

public class CustomerPostViewModel : CustomerBaseViewModel
{
  public IList<AddressPostViewModel> Addresses { get; set; }
}
