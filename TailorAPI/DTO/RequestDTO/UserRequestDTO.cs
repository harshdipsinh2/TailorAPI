namespace TailorAPI.DTO
{
    public class UserRequestDto
    {
        public string Name { get; set; }



        public int? ShopId { get; set; }
        public int? BranchId { get; set; }
        public string ShopName { get; set; }
        public string ShopLocation { get; set; }
        public string BranchName { get; set; }
        public string BranchLocation { get; set; }


        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
