using System.ComponentModel.DataAnnotations;

namespace TailorAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Key]
        public int ProductID { get; set; }  // Manually assigned ProductID

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }  // Price of the product
    }

}
