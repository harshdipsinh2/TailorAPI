namespace TailorAPI.Controllers;


using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class MeasurementController : ControllerBase
{
    private readonly MeasurementService _measurementService;

    public MeasurementController(MeasurementService measurementService)
    {
        _measurementService = measurementService;
    }

    [HttpPost]
    public async Task<IActionResult> AddMeasurement([FromBody] MeasurementDTO measurementDto)
    {
        try
        {
            var measurement = await _measurementService.AddMeasurementAsync(measurementDto);
            return CreatedAtAction(nameof(GetMeasurementByCustomerID), new { customerId = measurement.CustomerID }, measurement);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetMeasurementByCustomerID(int customerId)
    {
        var measurement = await _measurementService.GetMeasurementByCustomerIDAsync(customerId);
        if (measurement == null) return NotFound("Measurement not found.");
        return Ok(measurement);
    }

    [HttpDelete("{customerId}")]
    [HttpDelete]
    public async Task<IActionResult> SoftDeleteMeasurement([FromQuery] int measurementId)
    {
        var result = await _measurementService.SoftDeleteMeasurementAsync(measurementId);
        if (!result) return NotFound("Measurement not found.");

        return NoContent();
    }

}
