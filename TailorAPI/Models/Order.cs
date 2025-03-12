using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TailorAPI.DTOs.Request;
using TailorAPI.DTOs.Response;
using TailorAPI.Models;
using TailorAPI.Repositories;


namespace TailorAPI.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Fabric")]
        public int FabricID { get; set; }
        public Fabric Fabric { get; set; }

        [Required]
        public decimal FabricLength { get; set; }

        [Required]
        public int Quantity { get; set; } // ✅ Added Quantity for multiple item tracking

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; } = DateTime.Now.Date; // Auto-generated date-only format

        [Required]
        [Column(TypeName = "date")]
        public DateTime CompletionDate { get; set; } // Entered manually by the user

        public bool IsDeleted { get; set; } = false;  // Soft delete flag


    }
}
