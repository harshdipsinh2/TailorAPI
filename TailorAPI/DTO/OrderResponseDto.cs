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
        public DateTime OrderDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
