using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TailorAPI.Models
{
    public class Fabric
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ✅ Auto-generate FabricID in SQL
        public int FabricID { get; set; }

        [Required]
        public string FabricName { get; set; }

        [Required]
        public decimal PricePerMeter { get; set; } // Fabric cost per meter
        
        public decimal? StockQuantity { get; set; } // Optional for tracking inventory

        [Required]
        public decimal FabricUsed { get; set; } // Tracks the total fabric used

        public bool IsDeleted { get; set; } = false; // Soft delete flag

        // Navigation property for Orders
        public ICollection<Order> Orders { get; set; }
    }
}
