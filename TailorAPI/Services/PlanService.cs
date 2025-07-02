using Stripe.Checkout;
using TailorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TailorAPI.Services.Interface;
using Stripe;
using TailorAPI.DTO.RequestDTO;

public class PlanService : IPlanService
{
    private readonly TailorDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public PlanService(TailorDbContext context, IHttpContextAccessor httpContextAccessor , IConfiguration configuration)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;

    }

    public async Task<string> CreatePlanCheckoutSessionAsync(int planId)
    {
        var user = _httpContextAccessor.HttpContext.User;
        var shopId = int.Parse(user.FindFirst("shopId")?.Value ?? "0");
        var email = user.FindFirst(ClaimTypes.Email)?.Value;

        var plan = await _context.Plans.FirstOrDefaultAsync(p => p.PlanId == planId && p.IsActive);
        if (plan == null) throw new Exception("Invalid plan selected");

        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = plan.StripePriceId,
                    Quantity = 1
                }
            },
            Metadata = new Dictionary<string, string>
            {
                { "shopId", shopId.ToString() },
                { "planId", plan.PlanId.ToString() }
            },
            CustomerEmail = email,
            SuccessUrl = "https://yourdomain.com/success",
            CancelUrl = "https://yourdomain.com/cancel"
        };

        var stripeKey = _configuration["Stripe:SecretKey"];
        var client = new StripeClient(stripeKey);
        var service = new SessionService(client);
        var session = await service.CreateAsync(options);

        return session.Url;
    }

    public async Task<IEnumerable<Plan>> GetAllPlansAsync()
    {
        return await _context.Plans
            .Where(p => p.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }
    //public async Task<PlanCreateDTO> CreatePlanAsync(PlanCreateDTO dto)
    //{
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

    //    _context.Plans.Add(plan);
    //    await _context.SaveChangesAsync();
    //    return dto;
    //}


}