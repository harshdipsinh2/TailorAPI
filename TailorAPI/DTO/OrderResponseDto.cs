using System.ComponentModel.DataAnnotations.Schema;

namespace TailorAPI.DTO
{
    public class OrderResponseDto
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderDate { get; set; }
        public string CompletionDate { get; set; }
        //public string AssignedUserName { get; set; } 
    }
}
