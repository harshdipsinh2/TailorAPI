using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.DTOs.Request;
using TailorAPI.Services;
using TailorAPI.Services.Interface;

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")]  
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        private readonly IMeasurementService _measurementService;
        private readonly IProductService _productService;



        public AdminController(IAdminService adminService, 
                               ICustomerService customerService, 
                               IMeasurementService measurementService,
                               IProductService productService)

            
        {
            _adminService = adminService;
            _customerService = customerService;
            _measurementService = measurementService;
            _productService = productService;
        }

        // ----------- Customer Endpoints -----------

        
        /// Get all customers. Available to Admin, Manager, 
        [HttpGet("GetAllCustomers")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        /// Get customer details by ID. Available to Admin and Manager only.
        [HttpGet("GetCustomer")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<ActionResult<CustomerDTO>> GetCustomerById([FromQuery] int customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        /// Add a new customer. Available to Admin and Manager only.
        [HttpPost("AddCustomer")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<ActionResult<CustomerDTO>> PostCustomer([FromBody] CustomerRequestDTO customerDto)
        {
            if (customerDto == null)
                return BadRequest("Invalid customer data");

            var createdCustomer = await _customerService.AddCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { customerId = createdCustomer.FullName }, createdCustomer);
        }

        /// Update an existing customer. Available to Admin and Manager only.
        [HttpPut("EditCustomer")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<IActionResult> UpdateCustomer([FromQuery] int customerId, [FromBody] CustomerRequestDTO customerDto)
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(customerId, customerDto);
            if (updatedCustomer == null) return NotFound();
            return Ok(updatedCustomer);
        }

        /// Soft delete a customer. Available to Admin and Manager only.
        [HttpDelete("DeleteCustomer")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<IActionResult> SoftDeleteCustomer([FromQuery] int customerId)
        {
            var result = await _customerService.SoftDeleteCustomerAsync(customerId);
            if (!result) return NotFound("Customer not found.");
            return NoContent();
        }

        // ----------- Measurement Endpoints -----------

        /// Add a measurement for a customer. Available to Admin and Manager only.
        [HttpPost("AddMeasurement")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<IActionResult> AddMeasurement([FromQuery] int customerId, [FromBody] MeasurementRequestDTO measurementDto)
        {
            try
            {
                var measurement = await _measurementService.AddMeasurementAsync(customerId, measurementDto);
                return Ok(measurement);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Get measurement details by customer ID. Available to Admin and Manager only.
        [HttpGet("GetMeasurement")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<IActionResult> GetMeasurementByCustomerID([FromQuery] int customerId)
        {
            var measurement = await _measurementService.GetMeasurementByCustomerIDAsync(customerId);
            if (measurement == null) return NotFound("Measurement not found.");
            return Ok(measurement);
        }

        /// Soft delete a measurement. Available to Admin and Manager only.
        [HttpDelete("DeleteMeasurement")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SoftDeleteMeasurement([FromQuery] int measurementId)
        {
            var result = await _measurementService.SoftDeleteMeasurementAsync(measurementId);
            if (!result) return NotFound("Measurement not found.");

            return NoContent();
        }

        /// Get all measurements. Available to Admin and Manager only.
        [HttpGet("GetAllMeasurements")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<IActionResult> GetAllMeasurements()
        {
            var measurements = await _measurementService.GetAllMeasurementsAsync();
            if (measurements == null || measurements.Count == 0)
                return NotFound("No measurements found.");

            return Ok(measurements);

        }

        // ----------- Product Endpoints -----------

        /// Add a new product. Available to Admin and Manager only.
        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequestDTO productDto)
        {
            var result = await _productService.AddProduct(productDto);
            return Ok(result);
        }

        /// Update an existing product by ID. Available to Admin and Manager only.
        [HttpPut("UpdateProduct/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequestDTO productDto)
        {
            var result = await _productService.UpdateProduct(id, productDto);
            if (result == null) return NotFound("Product not found.");
            return Ok(result);
        }

        /// Delete a product by ID. Available to Admin and Manager only.
        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProduct(id);
            if (!success) return NotFound("Product not found.");
            return NoContent();
        }

        /// Get product details by ID. Available to Admin, Manager, and Tailor.
        [HttpGet("GetProduct/{id}")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductById(id);
            if (result == null) return NotFound("Product not found.");
            return Ok(result);
        }

        /// Get all products. Available to Admin, Manager, and Tailor.
        [HttpGet("GetAllProducts")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        }

    }
}