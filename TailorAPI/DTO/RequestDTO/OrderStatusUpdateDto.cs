using TailorAPI.Models;

namespace TailorAPI.DTO.Request
{
    public class OrderStatusUpdateDto
    {
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
