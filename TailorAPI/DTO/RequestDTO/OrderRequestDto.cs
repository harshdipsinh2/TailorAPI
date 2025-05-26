using TailorAPI.Models;

namespace TailorAPI.DTOs.Request
{
    public class OrderRequestDto
    {
        public double FabricLength { get; set; }
        public int Quantity { get; set; }
        public DateTime? CompletionDate { get; set; }

        public decimal TotalPrice { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public OrderApprovalStatus ApprovalStatus { get; set; }

    }
}
