using TailorAPI.Models;

namespace TailorAPI.DTO.ResponseDTO
{
    public class TailorResponseDTO
    {

        public int OrderID { get; set; }

        public SmsType SmsType { get; set; }
        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;


    }
}
