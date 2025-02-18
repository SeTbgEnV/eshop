using Microsoft.AspNetCore.Mvc;

namespace eshop.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(IUnitOfWork unitOfWork) : ControllerBase
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  [HttpGet()]
  public async Task<ActionResult> GetAllCustomers()
  {
    var customers = await _unitOfWork.CustomerRepository.List();
    return Ok(new { success = true, data = customers });
  }

  [HttpGet("{id}")]
  public async Task<ActionResult> GetCustomer(int id)
  {
    try
    {
      return Ok(new { success = true, data = await _unitOfWork.CustomerRepository.Find(id) });
    }
    catch (Exception ex)
    {
      return NotFound(new { success = false, message = ex.Message });
    }
  }

  [HttpPost()]
  public async Task<ActionResult> AddCustomer(CustomerPostViewModel model)
  {
    try
    {
      var result = await _unitOfWork.CustomerRepository.Add(model);
      if (result)
      {
        if (await _unitOfWork.Complete())
        {
          return StatusCode(201);
        }
        else
        {
          return StatusCode(500);
        }
      }
      else
      {
        return BadRequest(new { success = false, message = "Failed to add customer." });
      }
    }
    catch (Exception ex)
    {
      return BadRequest(new { success = false, message = ex.Message });
    }
  }
}