using TailorAPI.Models;

namespace TailorAPI.DTO.RequestDTO
{
    public class TwilioRequestDTO
    {
        public int OrderID { get; set; }


        public string BranchName { get; set; }
        public string ShopName { get; set; }



        public SmsType SmsType { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

    }
}
