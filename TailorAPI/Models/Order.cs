using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TailorAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {

        [Required]
        public string OrderNumber { get; set; }
        [Key]
        public int OrderID { get; set; }  // Auto-generated primary key

        [Required]
        public int CustomerID { get; set; }  // Foreign key (Customer)

        [Required]
        public int ProductID { get; set; }  // Foreign key (Product)

        [Required]
        public int EmployeeID { get; set; }  // Foreign key (Employee)

        [Required]
        public int Quantity { get; set; }  // Number of items ordered

        public decimal TotalPrice { get; set; }  // Calculated as (Price * Quantity)

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        //[ForeignKey("EmployeeID")]
        //public virtual Employee Employee { get; set; }
    }

}
