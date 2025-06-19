
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TailorAPI.Models
{
    public enum SmsType
    {
        PreCompletion,
        Completion,
        Delayed
    }
    public class TwilioSms
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TwilioSmsID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        public Order Order { get; set; }

        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [EnumDataType(typeof(SmsType))]
        [Column(TypeName = "nvarchar(24)")]
        public SmsType SmsType { get; set; } = SmsType.Completion;




        [ForeignKey("Branch")]
        public int BranchId { get; set; } // ✅ Foreign key to Branch
        public Branch Branch { get; set; } // Navigation property
        [ForeignKey("Shop")]
        public int ShopId { get; set; } // ✅ Foreign key to Shop
        public Shop Shop { get; set; } // Navigation property



    }
}

