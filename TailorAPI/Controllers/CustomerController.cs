using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    // ✅ 1. Get all customers
    [HttpGet("GetAllCustomers")]
    public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    // ✅ 2. Get customer by ID (Using FromQuery)
    [HttpGet("GetCustomer")]
    public async Task<ActionResult<CustomerDTO>> GetCustomerById([FromQuery] int customerId)  // ✅ Using FromQuery
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    // ✅ 3. Create new customer
    [HttpPost("AddCustomer")]
    public async Task<ActionResult<CustomerDTO>> PostCustomer([FromBody] CustomerDTO customerDto)
    {
        if (customerDto == null)
            return BadRequest("Invalid customer data");

        var createdCustomer = await _customerService.AddCustomerAsync(customerDto);
        return CreatedAtAction(nameof(GetCustomerById), new { customerId = createdCustomer.FullName }, createdCustomer); // ✅ Returns DTO, not full entity
    }


    // ✅ 4. Update existing customer
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromQuery] int customerId, [FromBody] CustomerDTO customerDto)
    {
        var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, customerDto);
        if (updatedCustomer == null) return NotFound();
        return Ok(updatedCustomer);
    }

    // ✅ 5. Delete customer
    // ✅ 5. Soft Delete customer
    [HttpDelete]
    public async Task<IActionResult> SoftDeleteCustomer([FromQuery] int customerId)
    {
        var result = await _customerService.SoftDeleteCustomerAsync(customerId);
        if (!result) return NotFound("Customer not found.");

        return NoContent();
    }

}
