using eshop.api.Entities;
using eshop.api.ViewModels.Address;

namespace eshop.api;

public interface IAddressRepository
{
  public Task<Address> Add(AddressPostViewModel model);
  public Task<bool> Add(string type);
}
