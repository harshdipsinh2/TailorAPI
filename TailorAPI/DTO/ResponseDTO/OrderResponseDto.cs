using System.ComponentModel.DataAnnotations;
using TailorAPI.Models;

public class OrderResponseDto
{
    public int OrderID { get; set; }

    public int CustomerID { get; set; }   // ✅ Added ID for reference
    public int ProductID { get; set; }    // ✅ Added ID for reference

    //public int FabricID { get; set; }     // ✅ Added ID for reference
    public int FabricTypeID { get; set; }


    public string? CustomerName { get; set; } // Nullable in case data is missing
    public string? ProductName { get; set; }

    //public string? FabricName { get; set; }
    public string? FabricName { get; set; }

    public decimal FabricLength { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public string? OrderDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public string? CompletionDate { get; set; }

    public int AssignedTo { get; set; }
    public string? AssignedToName { get; set; } // New property for Assigned User's Name
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;


    public OrderApprovalStatus ApprovalStatus { get; set; }
    public string? RejectionReason { get; set; }
}

