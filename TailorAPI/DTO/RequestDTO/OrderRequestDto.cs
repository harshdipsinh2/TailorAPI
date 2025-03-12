namespace TailorAPI.DTOs.Request
{
    public class OrderRequestDto
    {
        public double FabricLength { get; set; }
        public int Quantity { get; set; }
        public DateTime CompletionDate { get; set; }
    }
}