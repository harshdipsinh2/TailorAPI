using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TailorAPI.Models
{
    public class OtpVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // ✅ Auto-generate in SQL
        public int OtpId { get; set; }

        [ForeignKey("FabricType")]
        public int UserID { get; set; }
        public User user { get; set; }

        [Required]
        public string Otp { get; set; }

        public DateTime OtpExpiry { get; set; }

    }
}
