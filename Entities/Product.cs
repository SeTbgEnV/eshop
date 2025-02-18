namespace eshop.api.Entities;

public class Product
{
  public int Id { get; set; }
  public string ItemNumber { get; set; }
  public string ProductName { get; set; }
  public string Description { get; set; }
  public double Price { get; set; }
  public string Image { get; set; }

  // Navigational property...
  public IList<OrderItem> OrderItems { get; set; }
  public SupplierProduct SupplierProduct { get; set; }
}
