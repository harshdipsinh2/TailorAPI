using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.DTOs.Request;

namespace TailorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")]
    public class PaymentsController : ControllerBase
    {
        public PaymentsController(IConfiguration config)
        {
            // Pull your secret key from appsettings.json under "Stripe:SecretKey"
            StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreatePaymentDto dto)
        {
            // 1) Build the line-item for Stripe Checkout
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = dto.Currency,
                            UnitAmount = (long)(dto.TotalAmount * 100),  // amount in cents
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = dto.Description
                            },
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = dto.SuccessUrl,
                CancelUrl = dto.CancelUrl
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Ok(new { url = session.Url });
        }
    }
}
