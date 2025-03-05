using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TailorAPI.Models;

public class Order
{
    [Key]
    public int OrderID { get; set; }
    [ForeignKey("CustomerID")]
    public int CustomerID { get; set; }
    [ForeignKey("ProductID")]
    public int ProductID { get; set; }  
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; } // Auto-calculated
    public string OrderStatus { get; set; } = "Pending"; // Default
    public string PaymentStatus { get; set; } = "Pending"; // Default
    public DateTime OrderDate { get; set; } 
    public DateTime? CompletionDate { get; set; } // New field for order completion date


    //public int? AssignedTo { get; set; }  // Nullable (Order may not be assigned initially)

    //[ForeignKey("AssignedTo")]
    //public User? AssignedUser { get; set; }


    public Customer Customer { get; set; }
    public Product Product { get; set; }
}