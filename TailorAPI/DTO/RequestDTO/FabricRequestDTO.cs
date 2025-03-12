namespace TailorAPI.DTOs.Request
{
    public class FabricRequestDTO
    {
        public string FabricName { get; set; }
        public decimal PricePerMeter { get; set; }
        public decimal? StockQuantity { get; set; } // Optional
    }
}