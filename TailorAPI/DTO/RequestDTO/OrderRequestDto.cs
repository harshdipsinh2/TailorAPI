namespace TailorAPI.DTOs.Request
{
    public class OrderRequestDto
    {
        public double FabricLength { get; set; }
        public int Quantity { get; set; }
        public DateTime CompletionDate { get; set; }
        public int AssignedTo { get; set; }  // ✅ Added AssignedTo property
        public decimal TotalPrice { get; set; }

    }
}
