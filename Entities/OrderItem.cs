namespace eshop.api.Entities;

public class OrderItem
{
  public int SalesOrderId { get; set; }
  public int ProductId { get; set; }
  public int Quantity { get; set; }
  public double Price { get; set; }

  // Navigational properties...
  public Product Product { get; set; }
  public SalesOrder SalesOrder { get; set; }
}
