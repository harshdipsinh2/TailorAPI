using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailorAPI.Models
{
    // Enums for OrderStatus and PaymentStatus
    public enum OrderStatus
    {
        Pending,
        Completed
    }

    public enum PaymentStatus
    {
        Pending,
        Completed
    }
    public enum OrderApprovalStatus
    {
        Pending,    // Waiting for approval
        Approved,   // Ready for work
        Rejected    // Tailor declined
    }
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

        //[ForeignKey("Fabric")]
        //public int FabricID { get; set; }
        //public Fabric Fabric { get; set; }

        [ForeignKey("FabricType")]
        public int FabricTypeID { get; set; }
        public FabricType fabricType { get; set; }

        [Required]
        public decimal FabricLength { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public int AssignedTo { get; set; }

        [ForeignKey("AssignedTo")]
        public User Assigned { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; } = DateTime.Now.Date;

        [Required]
        [Column(TypeName = "date")]
        public DateTime? CompletionDate { get; set; }

        [Column(TypeName = "nvarchar(10)")] // ✅ Stored as string in SQL
        [JsonConverter(typeof(JsonStringEnumConverter))] // ✅ Displayed as string in JSON
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "nvarchar(10)")] // ✅ Stored as string in SQL
        [JsonConverter(typeof(JsonStringEnumConverter))] // ✅ Displayed as string in JSON
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        [Column(TypeName = "nvarchar(200)")]
        public string? RejectionReason { get; set; } // Required if rejected

        [Column(TypeName = "nvarchar(20)")]
        [JsonConverter(typeof(JsonStringEnumConverter))] // ✅ Displayed as string in JSON
        public OrderApprovalStatus ApprovalStatus { get; set; } = OrderApprovalStatus.Pending;

        public bool IsDeleted { get; set; } = false;
    }
}
