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

    //  Get all customers
    [HttpGet("GetAllCustomers")]
    public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    // Get customer by ID (Using FromQuery)
    [HttpGet("GetCustomer")]
    public async Task<ActionResult<CustomerDTO>> GetCustomerById([FromQuery] int customerId)  // Use FromQuery
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    //  Create new customer
    [HttpPost("AddCustomer")]
    public async Task<ActionResult<CustomerDTO>> PostCustomer([FromBody] CustomerDTO customerDto)
    {
        if (customerDto == null)
            return BadRequest("Invalid customer data");

        var createdCustomer = await _customerService.AddCustomerAsync(customerDto);
        return CreatedAtAction(nameof(GetCustomerById), new { customerId = createdCustomer.FullName }, createdCustomer); // ✅ Returns DTO, not full entity
    }


    //  Update existing customer
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromQuery] int customerId, [FromBody] CustomerDTO customerDto)
    {
        var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, customerDto);
        if (updatedCustomer == null) return NotFound();
        return Ok(updatedCustomer);
    }

    //  Delete customer
    //  Soft Delete customer
    [HttpDelete("Delete")]
    public async Task<IActionResult> SoftDeleteCustomer([FromQuery] int customerId)
    {
        var result = await _customerService.SoftDeleteCustomerAsync(customerId);
        if (!result) return NotFound("Customer not found.");

        return NoContent();
    }

}
