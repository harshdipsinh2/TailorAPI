using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ✅ Auto-generate in SQL
    public int CustomerID { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Address { get; set; }

    // Navigation Property (One-to-One)
    public Measurement Measurement { get; set; }
    //public List<Measurement> Measurements { get; set; } // Add this line
}


