namespace TailorAPI.DTO.RequestDTO
{
    public class AdminRegistrationDto
    {
        public string Name { get; set; }
        public string ShopName { get; set; }
        public string ShopLocation { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; } = "Admin";
    }
}
