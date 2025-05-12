namespace TailorAPI.DTOs.Request
{
    public class ProductRequestDTO
    {
        public string ProductName { get; set; }
        public decimal MakingPrice { get; set; }

        public string? ImageUrl { get; set; }

    }
}
