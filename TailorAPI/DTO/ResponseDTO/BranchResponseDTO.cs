namespace TailorAPI.DTO.ResponseDTO
{
    public class BranchResponseDTO
    {

        public string BranchName { get; set; }     
        public int BranchId { get; set; }


        public string Location { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // ✅ New field
        public int ShopId { get; set; }
        public string ShopName { get; set; }

    }
}
