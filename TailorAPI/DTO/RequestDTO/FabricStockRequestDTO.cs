namespace TailorAPI.DTO.RequestDTO
{
public class FabricStockRequestDTO
    {
        public int FabricTypeID { get; set; } // Add this property


        public string BranchName { get; set; }
        public string ShopName { get; set; }


        public decimal StockIn { get; set; }
        public DateTime StockAddDate { get; set; }
    }

}
