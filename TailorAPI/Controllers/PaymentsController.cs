using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.Models; // Include your Order model namespace if needed

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin,Manager")]
    public class PaymentsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly TailorDbContext _context;

        public PaymentsController(IConfiguration config, TailorDbContext context)
        {
            _config = config;
            _context = context;

            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession ([FromQuery] int orderId)

        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound("Order not found");

            var totalPrice = order.TotalPrice;
            var currency = "inr";
            var description = "Tailor Order Payment";

            var successUrl = "http://localhost:3000/payment-success";
            var cancelUrl = "https://localhost:3000/payment-cancel";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmount = (long)(totalPrice * 100), // Convert to paisa
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = description,
                            },
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = string.IsNullOrEmpty(successUrl)
                    ? "https://localhost:3000/payment-success"
                    : successUrl,
                CancelUrl = string.IsNullOrEmpty(cancelUrl)
                    ? "https://localhost:3000/payment-cancel"
                    : cancelUrl
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(new { url = session.Url });
        }
    }
}
