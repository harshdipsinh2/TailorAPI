namespace TailorAPI.DTO
{
    public class UserResponseDto
    {
        public int UserID { get; set; }


        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }


        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string RoleName { get; set; }
        public string UserStatus { get; set; }
        public bool IsVerified { get; set; }
    }
}
