using System.ComponentModel.DataAnnotations.Schema;

namespace eshop.api.Entities;

public class SupplierProduct
{
  public int ProductId { get; set; }
  public int SupplierId { get; set; }
  public string ItemNumber { get; set; }
  [Column(TypeName = "decimal(18, 2)")]
  public decimal Price { get; set; }

  public Product Product { get; set; }
  public Supplier Supplier { get; set; }
}
