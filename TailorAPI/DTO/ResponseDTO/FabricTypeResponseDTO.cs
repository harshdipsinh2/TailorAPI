namespace TailorAPI.DTO.ResponseDTO
{
    public class FabricTypeResponseDTO
    {
        public int FabricTypeID { get; set; }

        public string FabricName { get; set; }
        public decimal PricePerMeter { get; set; }
        public decimal AvailableStock { get; set; } // Optional for tracking inventory
    }
}
