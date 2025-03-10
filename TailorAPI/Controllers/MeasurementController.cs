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

[HttpPost("AddMeasurement")]
public async Task<IActionResult> AddMeasurement([FromQuery] int customerId, [FromBody] MeasurementDTO measurementDto)
{
    try
    {
        // Assign the CustomerID from query parameter instead of the request body
        measurementDto.CustomerID = customerId;

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


    [HttpGet("Detail")]
    public async Task<IActionResult> GetMeasurementByCustomerID(int customerId)
    {
        var measurement = await _measurementService.GetMeasurementByCustomerIDAsync(customerId);
        if (measurement == null) return NotFound("Measurement not found.");
        return Ok(measurement);
    }

    //[HttpDelete("{customerId}")]
    //[HttpDelete]
    //public async Task<IActionResult> SoftDeleteMeasurement([FromQuery] int measurementId)
    //{
    //    var result = await _measurementService.SoftDeleteMeasurementAsync(measurementId);
    //    if (!result) return NotFound("Measurement not found.");

    //    return NoContent();
    //}
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
        if (measurements == null || measurements.Count == 0) return NotFound("No measurements found.");
        return Ok(measurements);
    }





}
