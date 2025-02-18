using eshop.api.ViewModels.Supplier;
using Microsoft.AspNetCore.Mvc;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController(IUnitOfWork unitOfWork) : ControllerBase
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  [HttpGet()]
  public async Task<IActionResult> ListAllSuppliers()
  {
    try
    {
      // Steg 3.
      return Ok(new { success = true, data = await _unitOfWork.SupplierRepository.List() });
    }
    catch (Exception ex)
    {
      return NotFound($"Tyvärr hittade vi inget {ex.Message}");
    }

  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetSupplier(int id)
  {
    // Steg 3.
    try
    {
      return Ok(new { success = true, data = await _unitOfWork.SupplierRepository.Find(id) });
    }
    catch (Exception ex)
    {
      return NotFound(new { success = false, message = ex.Message });
    }
  }

  [HttpPost()]
  public async Task<IActionResult> AddSuppliers(SupplierPostViewModel model)
  {
    if (await _unitOfWork.SupplierRepository.Add(model))
    {
      if (_unitOfWork.HasChanges())
      {
        await _unitOfWork.Complete();
      }
      return StatusCode(201);
    }
    else
    {
      return BadRequest();
    }
  }
}
