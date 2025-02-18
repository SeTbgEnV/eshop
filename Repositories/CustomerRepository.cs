
using eshop.api.Data;
using eshop.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace eshop.api;

public class CustomerRepository(DataContext context, IAddressRepository repo) : ICustomerRepository
{
  private readonly DataContext _context = context;
  private readonly IAddressRepository _repo = repo;

  public async Task<bool> Add(CustomerPostViewModel model)
  {
    try
    {
      if (await _context.Customers.FirstOrDefaultAsync(c => c.Email.ToLower().Trim()
        == model.Email.ToLower().Trim()) != null)
      {
        throw new EShopException("Kunden finns redan");
      }

      var customer = new Customer
      {
        FirstName = model.FirstName,
        LastName = model.LastName,
        Email = model.Email,
        Phone = model.Phone
      };

      await _context.AddAsync(customer);

      foreach (var a in model.Addresses)
      {
        var address = await _repo.Add(a);

        await _context.CustomerAddresses.AddAsync(new CustomerAddress
        {
          Address = address,
          Customer = customer
        });
      }
      // return await _context.SaveChangesAsync() > 0;
      return true;
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
  }

  public async Task<CustomerViewModel> Find(int id)
  {

    try
    {
      var customer = await _context.Customers
        .Where(c => c.Id == id)
        .Include(c => c.CustomerAddresses)
          .ThenInclude(c => c.Address)
          .ThenInclude(c => c.PostalAddress)
        .Include(c => c.CustomerAddresses)
          .ThenInclude(c => c.Address)
          .ThenInclude(c => c.AddressType)
        .SingleOrDefaultAsync();

      if (customer is null)
      {
        throw new EShopException($"Det finns ingen kund med id {id}");
      }

      var view = new CustomerViewModel
      {
        Id = customer.Id,
        FirstName = customer.FirstName,
        LastName = customer.LastName,
        Email = customer.Email,
        Phone = customer.Phone
      };

      var addresses = customer.CustomerAddresses.Select(c => new AddressViewModel
      {
        AddressLine = c.Address.AddressLine,
        PostalCode = c.Address.PostalAddress.PostalCode,
        City = c.Address.PostalAddress.City,
        AddressType = c.Address.AddressType.Value
      });

      view.Addresses = [.. addresses];
      return view;
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
  }

  public async Task<IList<CustomersViewModel>> List()
  {
    var response = await _context.Customers.ToListAsync();
    var customers = response.Select(c => new CustomersViewModel
    {
      Id = c.Id,
      FirstName = c.FirstName,
      LastName = c.LastName,
      Email = c.Email,
      Phone = c.Phone
    });

    return [.. customers];
  }
}
