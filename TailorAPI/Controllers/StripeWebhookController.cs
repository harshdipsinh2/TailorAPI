using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using TailorAPI.Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/webhook")]
public class WebhookController : ControllerBase
{
    private readonly TailorDbContext _context;
    private readonly ILogger<WebhookController> _logger;
    private readonly IConfiguration _configuration;

    public WebhookController(TailorDbContext context, ILogger<WebhookController> logger, IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    
    [HttpPost("stripe")]
    public async Task<IActionResult> HandleStripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var endpointSecret = _configuration["Stripe:WebhookSecret"];

        try
        {
            var signatureHeader = Request.Headers["Stripe-Signature"];
            var stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session?.Metadata == null)
                    return BadRequest("Missing metadata");

                int shopId = int.Parse(session.Metadata["shopId"]);
                int planId = int.Parse(session.Metadata["planId"]);

                var shop = await _context.Shops.FindAsync(shopId);
                if (shop == null)
                    return NotFound("Shop not found");

                var plan = await _context.Plans.FirstOrDefaultAsync(p => p.PlanId == planId);
                if (plan == null)
                    return NotFound("Plan not found");
                plan.IsActive = true;

                // ✅ Update Shop
                shop.PlanId = planId;
                shop.PlanStartDate = DateTime.UtcNow;
                shop.PlanEndDate = DateTime.UtcNow.AddMonths(1);

                // ✅ Update all Branches with PlanId
                var branches = await _context.Branches.Where(b => b.ShopId == shopId).ToListAsync();
                foreach (var branch in branches)
                {
                    branch.PlanId = planId;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation($"✅ Shop {shopId} successfully updated to PlanId {planId} via checkout session.");
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            _logger.LogError($"Stripe webhook error: {ex.Message}");
            return BadRequest();
        }
    }

}