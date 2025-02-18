using System.ComponentModel.DataAnnotations.Schema;

namespace eshop.api.Entities;

public class Address
{
  public int Id { get; set; }
  public int AddressTypeId { get; set; }
  public int PostalAddressId { get; set; }
  public string AddressLine { get; set; }

  // [ForeignKey("PostalAddressId")]
  public PostalAddress PostalAddress { get; set; }

  // [ForeignKey("AddressTypeId")]
  public AddressType AddressType { get; set; }

  public IList<CustomerAddress> CustomerAddresses { get; set; }
  public IList<SupplierAddress> SupplierAddresses { get; set; }
}
