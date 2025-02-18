using System.Linq.Expressions;
using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels.Address;
using Microsoft.EntityFrameworkCore;

namespace eshop.api;

public class AddressRepository(DataContext context) : IAddressRepository
{
  private readonly DataContext _context = context;

  public async Task<Address> Add(AddressPostViewModel model)
  {
    var postalAddress = await _context.PostalAddresses.FirstOrDefaultAsync(
          c => c.PostalCode.Replace(" ", "").Trim() == model.PostalCode.Replace(" ", "").Trim());

    if (postalAddress is null)
    {
      postalAddress = new PostalAddress
      {
        PostalCode = model.PostalCode.Replace(" ", "").Trim(),
        City = model.City.Trim()
      };
      await _context.PostalAddresses.AddAsync(postalAddress);
    }

    var address = await _context.Addresses.FirstOrDefaultAsync(
      c => c.AddressLine.Trim().ToLower() == model.AddressLine.Trim().ToLower());

    if (address is null)
    {
      address = new Address
      {
        AddressLine = model.AddressLine,
        AddressTypeId = (int)model.AddressType,
        PostalAddress = postalAddress
      };

      await _context.Addresses.AddAsync(address);
    }

    if (_context.ChangeTracker.HasChanges())
    {
      await _context.SaveChangesAsync();
    }

    return address;
  }

  public async Task<bool> Add(string type)
  {
    try
    {
      var exists = await _context.AddressTypes.FirstOrDefaultAsync(c => c.Value.ToLower() == type.ToLower());

      if (exists is not null) throw new EShopException("Address type already exists");
      var newType = new AddressType
      {
        Value = type
      };
      await _context.AddressTypes.AddAsync(newType);
      return true;
    }
    catch (Exception ex)
    {
      throw new Exception(ex.Message);
    }
  }
}
