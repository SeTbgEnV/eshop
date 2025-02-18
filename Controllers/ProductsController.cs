using eshop.api.Data;
using eshop.api.Entities;
using eshop.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(DataContext context) : ControllerBase
{
  private readonly DataContext _context = context;

  [HttpGet()]
  public async Task<ActionResult> ListAllProducts()
  {
    var products = await _context.Products
      .Select(product => new
      {
        product.Id,
        product.ItemNumber,
        product.ProductName,
        product.Description,
        product.Price,
        product.Image
      }
      )
      .ToListAsync();
    return Ok(new { success = true, products });
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> FindProduct(int id)
  {
    var product = await _context.Products
      .Select(product => new
      {
        product.Id,
        product.ItemNumber,
        product.ProductName,
        product.Description,
        product.Price,
        product.Image
      }
      )
      .SingleOrDefaultAsync(p => p.Id == id);

    if (product != null)
      return Ok(new { success = true, product });
    else
      return NotFound(new { success = false, message = $"Tyvärr kunde vi inte hitta någon produkt med id {id}" });
  }

  [HttpPost()]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult> AddProduct(ProductPostViewModel model)
  {
    // if (!User.IsInRole("Admin"))
    // {
    //   return StatusCode(403, new { success = false, message = "Du har inte behörighet!" });
    // }
    // Kontrollera så att produkten inte finns...
    var prod = await _context.Products.FirstOrDefaultAsync(p => p.ItemNumber == model.ItemNumber);

    if (prod != null)
    {
      return BadRequest(new { success = false, message = $"Produkten existerar redan {0}", model.ProductName });
    }

    // Annars lägg till ny produkt...
    // Mappa om ProductPostViewModel till Product entity...
    var product = new Product
    {
      ItemNumber = model.ItemNumber,
      ProductName = model.ProductName,
      Description = model.Description,
      Price = model.Price,
      Image = model.Image
    };

    try
    {
      await _context.Products.AddAsync(product);
      await _context.SaveChangesAsync();

      // return Ok(new {success=true, data = product});
      return CreatedAtAction(nameof(FindProduct), new { id = product.Id }, product);
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }
  }

  // [HttpPatch("{id}/{price}")]
  [HttpPatch("{id}")]
  [Authorize(Roles = "Admin")]
  // public async Task<ActionResult> UpdateProductPrice(int id, double price)
  public async Task<ActionResult> UpdateProductPrice(int id, [FromQuery] double price)
  {
    var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

    if (prod == null)
    {
      return NotFound(new { success = false, message = $"Produkten som du försöker uppdatera existerar inte längre {0}", id });
    }

    prod.Price = price;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
      return StatusCode(500, ex.Message);
    }

    return NoContent();
  }
}
