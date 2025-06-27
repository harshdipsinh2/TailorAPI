namespace TailorAPI.DTO.RequestDTO
{
    public class EmployeeRegistrationDto
    {
        public string Name { get; set; }
        public int ShopId { get; set; }
        public int BranchId { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; } // "Manager" or "Tailor"
    }
}
