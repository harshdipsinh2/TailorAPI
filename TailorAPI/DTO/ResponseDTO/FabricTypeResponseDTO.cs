namespace TailorAPI.DTO.ResponseDTO
{
    public class FabricTypeResponseDTO
    {
        public int FabricTypeID { get; set; }


        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }

        public string FabricName { get; set; }
        public decimal PricePerMeter { get; set; }
        public decimal AvailableStock { get; set; } // Optional for tracking inventory
    }
}
