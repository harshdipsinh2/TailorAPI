namespace TailorAPI.DTO.ResponseDTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public int UserID { get; set; }
    }
}
