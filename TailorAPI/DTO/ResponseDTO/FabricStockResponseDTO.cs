namespace TailorAPI.DTO.ResponseDTO
{
    public class FabricStockResponseDTO
    {
        public int StockID { get; set; }
        public int FabricTypeID { get; set; }


        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }

        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public DateTime StockAddDate { get; set; }

    }
}
