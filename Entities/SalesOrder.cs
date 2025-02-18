using System.ComponentModel.DataAnnotations;

namespace eshop.api.Entities;

public class SalesOrder
{
  [Key]
  public int SalesOrderId { get; set; }
  public DateTime OrderDate { get; set; }

  // Navigational property...
  public IList<OrderItem> OrderItems { get; set; }
}
