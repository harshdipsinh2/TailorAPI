using System.ComponentModel.DataAnnotations;

namespace TailorAPI.DTOs.Response
{
    public class OrderResponseDto
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string FabricName { get; set; }
        public decimal FabricLength { get; set; }
        public int Quantity { get; set; } // ✅ Added Quantity for multiple item tracking
        public decimal TotalPrice { get; set; }
        [Required]
        [DataType(DataType.Date)] // Ensures date-only format in Swagger UI
        public string CompletionDate { get; set; }  // ✅ Accept CompletionDate as string (date only)
    }
}
