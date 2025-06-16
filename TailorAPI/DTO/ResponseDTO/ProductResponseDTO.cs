using System.ComponentModel.DataAnnotations;
using TailorAPI.Models;

namespace TailorAPI.DTOs.Response
{
    public class ProductResponseDTO
    {
        public int ProductID { get; set; }  // Manually assigned ProductID

        public string ProductName { get; set; }


        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }


        public decimal MakingPrice { get; set; }  // Tailoring work price (excluding fabric cost)

        public ProductType ProductType { get; set; } // New field
        public string? ImageUrl { get; set; }


    }
}
