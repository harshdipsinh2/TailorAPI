using System.ComponentModel.DataAnnotations;

namespace TailorAPI.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        [Required]
        public string RoleName { get; set; }

        // Navigation Property
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }

}
