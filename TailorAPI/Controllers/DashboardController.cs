using Microsoft.AspNetCore.Mvc;
using TailorAPI.Services.Interface;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
   
    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    

    [HttpGet("summary")]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var summary = await _dashboardService.GetDashboardSummaryAsync();
        return Ok(summary);
    }
}
