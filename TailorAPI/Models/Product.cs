using System.ComponentModel.DataAnnotations;

namespace TailorAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }  // Manually assigned ProductID

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal MakingPrice { get; set; }  // Tailoring work price (excluding fabric cost)

        public bool IsDeleted { get; set; } = false;  // Soft delete property
    }
}
