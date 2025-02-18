using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize()]
public class OrdersController(DataContext context) : ControllerBase
{
  private readonly DataContext _context = context;

  [HttpGet()]
  public async Task<ActionResult> ListAllOrders()
  {
    // DEKLARATIV PROGRAMMERING❗️
    // När vi hämtar alla SalesOrder objekt
    // ta med dig alla OrderItems objekt också
    var orders = await _context.SalesOrders
      .Include(c => c.OrderItems) // Vi måste nu projicera datat till det format som vi vill leverera tillbaka...
      .Select(order => new
      {
        OrderNumber = order.SalesOrderId,
        order.OrderDate,
        OrderItems = order.OrderItems // Nu måste projicera vad vi vill/behöver ifrån OrderItems!!!
          .Select(item => new
          {
            item.Product.ProductName,
            item.Price,
            item.Quantity,
            LineSum = item.Price * item.Quantity
          })
      })
      .ToListAsync();

    return Ok(new { success = true, statusCode = 200, data = orders });
  }

  // Hämta ut all information om en specifik beställning baserat orderns id...
  [HttpGet("{id}")]
  public async Task<ActionResult> FindOrder(int id)
  {
    var order = await _context.SalesOrders
      .Where(o => o.SalesOrderId == id)
      .Include(c => c.OrderItems)
      .Select(order => new
      {
        OrderNumber = order.SalesOrderId,
        order.OrderDate,
        OrderItems = order.OrderItems
          .Select(item => new
          {
            item.Product.ProductName,
            item.Price,
            item.Quantity,
            LineSum = item.Price * item.Quantity
          })
      })
      .SingleOrDefaultAsync();

    if (order is null)
    {
      return NotFound(new { success = false, statusCode = 404, message = $"Tyvärr vi kunde inte hitta någon beställning med ordernummer: {id}" });
    }
    return Ok(new { success = true, statusCode = 200, data = order });
  }

  [HttpPost()]
  public async Task<ActionResult> AddOrder(SalesOrderViewModel order)
  {
    var newOrder = new SalesOrder
    {
      OrderDate = order.OrderDate,
      OrderItems = []
    };

    // TODO: Försäkra oss om att inte samma produkt är inskickad mer än en gång!!!

    foreach (var product in order.Products)
    {
      var prod = await _context.Products.SingleOrDefaultAsync(p => p.Id == product.ProductId);

      if (prod is null) return BadRequest($"Du har angivet ett produkt id som inte existerar");

      var item = new OrderItem
      {
        Price = product.Price,
        Quantity = product.Quantity,
        ProductId = product.ProductId
      };
      newOrder.OrderItems.Add(item);
    }

    try
    {
      await _context.SalesOrders.AddAsync(newOrder);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(FindOrder), new { id = newOrder.SalesOrderId }, new
      {
        newOrder.SalesOrderId,
        newOrder.OrderDate
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return BadRequest();
    }

  }

  [HttpPut("{id}")]
  public async Task<ActionResult> UpdateOrder(int id, SalesOrderViewModel order)
  {
    var orderToUpdate = await _context.SalesOrders
    .Where(c => c.SalesOrderId == id)
    .Include(o => o.OrderItems)
    .SingleOrDefaultAsync();

    if (orderToUpdate is null) return BadRequest($"Det finns ingen beställning med ordernummer {id}");

    orderToUpdate.OrderDate = order.OrderDate;

    foreach (var item in order.Products)
    {
      foreach (var orderItem in orderToUpdate.OrderItems)
      {
        orderItem.Price = item.Price;
        orderItem.Quantity = item.Quantity;
      }
    }

    await _context.SaveChangesAsync();
    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteOrder(int id)
  {
    var toDelete = await _context.SalesOrders.FindAsync(id);
    _context.SalesOrders.Remove(toDelete);
    await _context.SaveChangesAsync();

    return NoContent();
  }

}
