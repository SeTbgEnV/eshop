namespace eshop.api;

public interface ICustomerRepository
{
  public Task<IList<CustomersViewModel>> List();
  public Task<CustomerViewModel> Find(int id);
  public Task<bool> Add(CustomerPostViewModel model);
}
