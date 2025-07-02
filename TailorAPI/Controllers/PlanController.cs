using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO.RequestDTO;
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

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPlans()
    {
        var plans = await _planService.GetAllPlansAsync();
        return Ok(plans);
    }


    //[HttpPost("create")]
    //public async Task<IActionResult> CreatePlan([FromBody] PlanCreateDTO dto)
    //{
    //    if (dto == null) return BadRequest("Invalid plan data.");

    //    var plan = new Plan
    //    {
    //        Name = dto.Name,
    //        MaxBranches = dto.MaxBranches,
    //        MaxOrders = dto.MaxOrders,
    //        PricePerMonth = dto.PricePerMonth,
    //        StripeProductId = dto.StripeProductId,
    //        StripePriceId = dto.StripePriceId,
    //        IsActive = dto.IsActive
    //    };

    //    var createdPlan = await _planService.CreatePlanAsync(dto);
    //    return CreatedAtAction(nameof(GetAllPlans), new { id = createdPlan.PlanId }, createdPlan);
    //}

}