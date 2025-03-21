using System.ComponentModel.DataAnnotations;

namespace TailorAPI.Models
{
    public enum RoleType
    {
        Admin,
        Manager,
        Tailor
    }

    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public RoleType RoleType { get; set; } // New enum for easier role assignment

        // Navigation Property
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
