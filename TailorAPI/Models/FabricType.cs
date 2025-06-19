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




        [ForeignKey("Branch")]
        public int BranchId { get; set; } // ✅ Foreign key to Branch
        public Branch Branch { get; set; } // Navigation property
        [ForeignKey("Shop")]
        public int ShopId { get; set; } // ✅ Foreign key to Shop
        public Shop Shop { get; set; } // Navigation property




    }
}
