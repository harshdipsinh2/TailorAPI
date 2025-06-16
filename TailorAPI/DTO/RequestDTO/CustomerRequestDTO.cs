using System.Text.Json.Serialization;

namespace TailorAPI.DTO.RequestDTO
{
    public class CustomerRequestDTO
    {
        public string FullName { get; set; }


        public string BranchName { get; set; }
        public string ShopName { get; set; }


        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
    }
}
