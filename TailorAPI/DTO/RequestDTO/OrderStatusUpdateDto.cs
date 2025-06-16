using TailorAPI.Models;

namespace TailorAPI.DTO.Request
{
    public class OrderStatusUpdateDto
    {
        public OrderStatus OrderStatus { get; set; }


        public string BranchName { get; set; }
        public string ShopName { get; set; }


        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? CompletionDate { get; set; }

    }
}
