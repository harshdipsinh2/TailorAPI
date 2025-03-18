namespace TailorAPI.DTOs.Response
{
    public class FabricResponseDTO
    {
        public int FabricID { get; set; }

        public string FabricName { get; set; }
        public decimal PricePerMeter { get; set; }
        public decimal? StockQuantity { get; set; }
    }
}
