using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.Services.Interface;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PlanController : ControllerBase
{
    private readonly IPlanService _planService;

    public PlanController(IPlanService planService)
    {
        _planService = planService;
    }

    [HttpPost("buy")]
    public async Task<IActionResult> BuyPlan([FromQuery] int planId)
    {
        try
        {
            var sessionUrl = await _planService.CreatePlanCheckoutSessionAsync(planId);
            return Ok(new { url = sessionUrl });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}