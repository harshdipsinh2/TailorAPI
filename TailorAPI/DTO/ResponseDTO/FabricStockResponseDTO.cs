namespace TailorAPI.DTO.ResponseDTO
{
    public class FabricStockResponseDTO
    {
        public int StockID { get; set; }
        public int FabricTypeID { get; set; }

        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public DateTime StockAddDate { get; set; }

    }
}
