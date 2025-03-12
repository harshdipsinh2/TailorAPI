using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services.Interface;

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        [HttpPost("AddMeasurement")]
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



        [HttpGet("Detail")]
        public async Task<IActionResult> GetMeasurementByCustomerID([FromQuery] int customerId)
        {
            var measurement = await _measurementService.GetMeasurementByCustomerIDAsync(customerId);
            if (measurement == null) return NotFound("Measurement not found.");
            return Ok(measurement);
        }


        [HttpDelete]
        public async Task<IActionResult> SoftDeleteMeasurement([FromQuery] int measurementId)
        {
            var result = await _measurementService.SoftDeleteMeasurementAsync(measurementId);
            if (!result) return NotFound("Measurement not found.");

            return NoContent();
        }


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllMeasurements()
        {
            var measurements = await _measurementService.GetAllMeasurementsAsync();
            if (measurements == null || measurements.Count == 0)
                return NotFound("No measurements found.");

            return Ok(measurements);
        }
    }
}