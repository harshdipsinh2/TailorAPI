using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TailorAPI.Models;

public class Plan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PlanId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int MaxBranches { get; set; }

    [Required]
    public int MaxOrders { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePerMonth { get; set; }

    public string StripeProductId { get; set; } // NEW
    public string StripePriceId { get; set; }   // NEW

    public bool IsActive { get; set; } = true;

    public ICollection<Branch>? Branches { get; set; }
    public ICollection<Order>? Orders { get; set; }
}
