namespace TailorAPI.DTO
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; } // Rename to Password for clarity
    }
}
