using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TailorAPI.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate UserID
    public int UserID { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string MobileNo { get; set; }
    public string PasswordHash { get; set; }
    public string Address { get; set; }

    public int RoleID { get; set; } // Assigned automatically
    [ForeignKey("RoleID")]
    public Role Role { get; set; }
}
