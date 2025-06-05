namespace TailorAPI.DTO.ResponseDTO
{
    public class TwilioSmsResponseDTO
    {
        public int OrderID { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public string SmsType { get; set; }
    }
}
