using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Add this namespace


namespace TailorAPI.Models
{
    public class FabricType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FabricTypeID { get; set; }
        [Required]
        public string FabricName { get; set; }

        [Required]
        public decimal PricePerMeter { get; set; } // Fabric cost per meter

        public decimal AvailableStock { get; set; } // Optional for tracking inventory

        [Required]

        public bool IsDeleted { get; set; } = false; // Soft delete flag

        // Navigation property for Orders
        public ICollection<Order> Orders { get; set; }


    }
}
