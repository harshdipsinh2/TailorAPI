namespace TailorAPI.DTOs.Response
{
    public class ProductResponseDTO
    {
        public int ProductID { get; set; }  // Manually assigned ProductID

        public string ProductName { get; set; }

        public decimal MakingPrice { get; set; }  // Tailoring work price (excluding fabric cost)

    }
}
