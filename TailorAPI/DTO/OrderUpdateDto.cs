namespace TailorAPI.DTO
{
    public class OrderUpdateDto
    {
        public int Quantity { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? CompletionDate { get; set; }
    }

}
