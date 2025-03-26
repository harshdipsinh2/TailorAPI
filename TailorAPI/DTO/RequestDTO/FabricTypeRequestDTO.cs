namespace TailorAPI.DTO.RequestDTO
{
    public class FabricTypeRequestDTO
    {
        public string FabricName { get; set; }
        public decimal PricePerMeter { get; set; }
        public decimal AvailableStock { get; set; } // Optional for tracking inventory
    }
}
