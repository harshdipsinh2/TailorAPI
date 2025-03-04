public class Order
{
    public int OrderID { get; set; }
    public int CustomerID { get; set; } // FK
    public int ProductID { get; set; }  // FK
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; } // Auto-calculated
    public string OrderStatus { get; set; } = "Pending"; // Default
    public string PaymentStatus { get; set; } = "Pending"; // Default
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? CompletionDate { get; set; } // New field for order completion date
}