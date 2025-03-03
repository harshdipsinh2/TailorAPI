using System.ComponentModel.DataAnnotations;

namespace TailorAPI.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        // Navigation Property
        public virtual ICollection<User> Users { get; set; }
    }

}
