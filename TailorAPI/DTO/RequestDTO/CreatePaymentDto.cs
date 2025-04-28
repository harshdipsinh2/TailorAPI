// --- CreatePaymentDto.cs ---
namespace TailorAPI.DTOs.Request
{
    public class CreatePaymentDto
    {
        /// <summary>
        /// Total amount to charge (in major currency units, e.g. 20.50 for $20.50)
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Currency code (default "usd")
        /// </summary>
        public string Currency { get; set; } = "usd";

        /// <summary>
        /// Description shown on the Stripe Checkout page
        /// </summary>
        public string Description { get; set; } = "Order Payment";

        /// <summary>
        /// Where Stripe redirects on successful payment
        /// </summary>
        public string SuccessUrl { get; set; }

        /// <summary>
        /// Where Stripe redirects if the user cancels
        /// </summary>
        public string CancelUrl { get; set; }
    }
}
