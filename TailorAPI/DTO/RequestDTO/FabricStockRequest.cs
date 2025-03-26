namespace TailorAPI.DTO.RequestDTO
{
    public class FabricStockRequest
    {
        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public DateTime StockAddDate { get; set; }
    }
}
