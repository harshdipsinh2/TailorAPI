namespace TailorAPI.DTOs.Request
{
    public class CreatePaymentDto
    {
        public decimal TotalAmount { get; set; }


        public string Currency { get; set; } = "inr";

        public string Description { get; set; } = "Order Payment";

        public string SuccessUrl { get; set; }

        public string CancelUrl { get; set; }
    }
}

