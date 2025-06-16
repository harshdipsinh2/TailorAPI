using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TailorAPI.Models;

public enum Gender
{
    Male ,
    Female
}

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ✅ Auto-generate in SQL
    public int CustomerId { get; set; }



    [ForeignKey("Branch")]
    public int BranchId { get; set; } // ✅ Foreign key to Branch
    public Branch Branch { get; set; } // Navigation property
    [ForeignKey("Shop")]
    public int ShopId { get; set; } // ✅ Foreign key to Shop
    public Shop Shop { get; set; } // Navigation property



    [Required]
    public string FullName { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(10)")] // ✅ This will store Gender as a string
    [JsonConverter(typeof(JsonStringEnumConverter))] // ✅ This ensures JSON shows "Male"/"Female"
    public Gender Gender { get; set; }


    public bool IsDeleted { get; set; } = false; // ✅ Soft delete flag

    // Navigation Property (One-to-One)
    public Measurement Measurement { get; set; }
}
