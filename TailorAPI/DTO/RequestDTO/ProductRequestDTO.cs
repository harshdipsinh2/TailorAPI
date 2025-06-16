using System.ComponentModel.DataAnnotations;
using TailorAPI.Models;

namespace TailorAPI.DTOs.Request
{
    public class ProductRequestDTO
    {
        public string ProductName { get; set; }
        public decimal MakingPrice { get; set; }


        public string BranchName { get; set; }
        public string ShopName { get; set; }



        public ProductType ProductType { get; set; } // New field

        public string? ImageUrl { get; set; }

    }
}
