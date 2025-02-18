using eshop.api.ViewModels.Supplier;

namespace eshop.api;

public interface ISupplierRepository
{
  public Task<IList<SuppliersViewModel>> List();
  public Task<SupplierViewModel> Find(int id);
  public Task<bool> Add(SupplierPostViewModel model);
}
