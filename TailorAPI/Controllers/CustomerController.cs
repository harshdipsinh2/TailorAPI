using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("GetAllCustomers")]
    public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("GetCustomer")]
    public async Task<ActionResult<CustomerDTO>> GetCustomerById([FromQuery] int customerId)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost("AddCustomer")]
    public async Task<ActionResult<CustomerDTO>> PostCustomer([FromBody] CustomerRequestDTO customerDto)
    {
        if (customerDto == null)
            return BadRequest("Invalid customer data");

        var createdCustomer = await _customerService.AddCustomerAsync(customerDto);
        return CreatedAtAction(nameof(GetCustomerById), new { customerId = createdCustomer.FullName }, createdCustomer);
    }

    [HttpPut("Edit")]
    public async Task<IActionResult> UpdateCustomer([FromQuery] int customerId, [FromBody] CustomerRequestDTO customerDto)
    {
        var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, customerDto);
        if (updatedCustomer == null) return NotFound();
        return Ok(updatedCustomer);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> SoftDeleteCustomer([FromQuery] int customerId)
    {
        var result = await _customerService.SoftDeleteCustomerAsync(customerId);
        if (!result) return NotFound("Customer not found.");

        return NoContent();
    }
}
