namespace TailorAPI.DTO.RequestDTO
{
public class FabricStockRequestDTO
    {
        public int FabricTypeID { get; set; } // Add this property
        public decimal StockIn { get; set; }
        public DateTime StockAddDate { get; set; }
    }

}
