using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate EmployeeID
    public int EmployeeID { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Address { get; set; }

    [Required]
    public string Role { get; set; } // Role assigned during registration

    [Column(TypeName = "decimal(18,2)")] // ✅ Define precision explicitly
    public decimal Salary { get; set; } // Monthly Salary

    public int Attendance { get; set; } // Total Present Days

    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active; // Enum for better control

    public int? CustomerID { get; set; }  // Assigned Customer (Nullable)
    public int? MeasurementID { get; set; } // Assigned Measurement (Nullable)

    public bool IsDeleted { get; set; } = false; // ✅ Soft delete flag

    // Navigation properties (for Entity Framework relations)
    public virtual Customer Customer { get; set; }
    public virtual Measurement Measurement { get; set; }

    public string? IdentityUserId { get; set; }

}

// ✅ Enum for Status (Prevents typo issues)
public enum EmployeeStatus
{
    Active,
    Inactive
}
