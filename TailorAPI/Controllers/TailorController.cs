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
    [Authorize(Roles = "Admin,Manager,Tailor")]
    public class TailorController : ControllerBase
    {
        
        private readonly ICustomerService _customerService;
        private readonly IMeasurementService _measurementService;
        private readonly IProductService _productService;



        public TailorController(ICustomerService customerService,
                               IMeasurementService measurementService,
                               IProductService productService)


        {
            _customerService = customerService;
            _measurementService = measurementService;
            _productService = productService;
        }


        [HttpPost("AddMeasurement")]
        [Authorize(Roles = "Tailor")]
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

        [HttpGet("GetAllMeasurements")]
        [Authorize(Roles = "Tailor")]
        public async Task<IActionResult> GetAllMeasurements()
        {
            var measurements = await _measurementService.GetAllMeasurementsAsync();
            if (measurements == null || measurements.Count == 0)
                return NotFound("No measurements found.");

            return Ok(measurements);

        }

        /// Get all products. Available to Admin, Manager, and Tailor.
        [HttpGet("GetAllProducts")]
        [Authorize(Roles = "Tailor")]

        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        }


    }
}
