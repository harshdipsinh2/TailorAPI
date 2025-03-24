//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using TailorAPI.DTO.RequestDTO;
//using TailorAPI.DTO.ResponseDTO;
//using TailorAPI.Services.Interface;

//namespace TailorAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(Roles = "Admin")]
//    public class AdminController : ControllerBase
//    {
//        private readonly IAdminService _adminService;

//        public AdminController(IAdminService adminService)
//        {
//            _adminService = adminService;
//        }

//        // GET ALL CUSTOMERS
//        [HttpGet("customers")]
//        public async Task<IActionResult> GetAllCustomers()
//        {
//            var customers = await _adminService.GetAllCustomersAsync();
//            return Ok(customers);
//        }

//        // GET CUSTOMER BY ID
//        [HttpGet("customers/{id}")]
//        public async Task<IActionResult> GetCustomerById(int id)
//        {
//            var customer = await _adminService.GetCustomerByIdAsync(id);
//            if (customer == null) return NotFound("Customer not found.");
//            return Ok(customer);
//        }

//        // ADD CUSTOMER
//        [HttpPost("customers")]
//        public async Task<IActionResult> AddCustomer([FromBody] CustomerRequestDTO customerDto)
//        {
//            var newCustomer = await _adminService.AddCustomerAsync(customerDto);
//            return CreatedAtAction(nameof(GetCustomerById), new { id = newCustomer.CustomerId }, newCustomer);
//        }

//        // UPDATE CUSTOMER
//        [HttpPut("customers/{id}")]
//        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerRequestDTO customerDto)
//        {
//            var updatedCustomer = await _adminService.UpdateCustomerAsync(id, customerDto);
//            if (updatedCustomer == null) return NotFound("Customer not found.");
//            return Ok(updatedCustomer);
//        }

//        // DELETE CUSTOMER (Soft Delete)
//        [HttpDelete("customers/{id}")]
//        public async Task<IActionResult> SoftDeleteCustomer(int id)
//        {
//            var success = await _adminService.SoftDeleteCustomerAsync(id);
//            if (!success) return NotFound("Customer not found.");
//            return Ok("Customer successfully deleted.");
//        }

//        // ADD MEASUREMENT
//        [HttpPost("measurements/{customerId}")]
//        public async Task<IActionResult> AddMeasurement(int customerId, [FromBody] MeasurementRequestDTO measurementDto)
//        {
//            var measurement = await _adminService.AddMeasurementAsync(customerId, measurementDto);
//            return Ok(measurement);
//        }

//        // GET MEASUREMENT BY CUSTOMER ID
//        [HttpGet("measurements/{customerId}")]
//        public async Task<IActionResult> GetMeasurementByCustomerID(int customerId)
//        {
//            var measurement = await _adminService.GetMeasurementByCustomerIDAsync(customerId);
//            if (measurement == null) return NotFound("Measurement not found.");
//            return Ok(measurement);
//        }

//        // DELETE MEASUREMENT (Soft Delete)
//        [HttpDelete("measurements/{measurementId}")]
//        public async Task<IActionResult> SoftDeleteMeasurement(int measurementId)
//        {
//            var success = await _adminService.SoftDeleteMeasurementAsync(measurementId);
//            if (!success) return NotFound("Measurement not found.");
//            return Ok("Measurement successfully deleted.");
//        }

//        // GET ALL MEASUREMENTS
//        [HttpGet("measurements")]
//        public async Task<IActionResult> GetAllMeasurements()
//        {
//            var measurements = await _adminService.GetAllMeasurementsAsync();
//            return Ok(measurements);
//        }
//    }
//}