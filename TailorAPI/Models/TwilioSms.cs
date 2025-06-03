
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

        public SmsType SmsType { get; set; } = SmsType.Completion;


    }
}

