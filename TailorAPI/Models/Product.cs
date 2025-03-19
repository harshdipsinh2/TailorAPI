using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TailorAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }  

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal MakingPrice { get; set; }  // Tailoring work price (excluding fabric cost)

        public bool IsDeleted { get; set; } = false;  // Soft delete property
        
        // Navigation property for Orders
        public ICollection<Order> Orders { get; set; }
    }
}
