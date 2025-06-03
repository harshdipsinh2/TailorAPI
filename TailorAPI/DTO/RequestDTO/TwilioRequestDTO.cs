using TailorAPI.Models;

namespace TailorAPI.DTO.RequestDTO
{
    public class TwilioRequestDTO
    {
        public int OrderID { get; set; }

        public SmsType SmsType { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

    }
}
