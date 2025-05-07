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
    [Authorize(Roles = "Tailor")]
    public class TailorController : ControllerBase
    {

        private readonly ICustomerService _customerService;
        private readonly IMeasurementService _measurementService;
        private readonly IProductService _productService;
        private readonly IDashboardService _dashboardService;
        private readonly IFabricCombinedService _fabricCombinedService;
        private readonly IOrderService _orderService;


        public TailorController(ICustomerService customerService,
                               IMeasurementService measurementService,
                               IProductService productService,
                               IDashboardService dashboardService,
                               IFabricCombinedService fabricCombinedService,
                               IOrderService orderService)


        {
            _orderService = orderService;
            _dashboardService = dashboardService;
            _customerService = customerService;
            _measurementService = measurementService;
            _productService = productService;
            _fabricCombinedService = fabricCombinedService;
        }

        //-------------------Dashboard end points ---------------------
        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var summary = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(summary);
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

        [HttpGet("GetAllFabricTypes")]
        public async Task<IActionResult> GetAllFabricTypes()
        {
            var result = await _fabricCombinedService.GetAllFabricTypesAsync();
            return Ok(result);
        }

        [HttpGet("GetAllFabricStocks")]
        public async Task<IActionResult> GetAllFabricStocks()
        {
            var result = await _fabricCombinedService.GetAllFabricStocksAsync();
            return Ok(result);
        }

        [HttpGet("GetAll-Order")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

    }
}
