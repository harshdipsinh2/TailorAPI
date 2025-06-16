using System.ComponentModel.DataAnnotations;
using TailorAPI.Models;

namespace TailorAPI.DTO.RequestDTO
{
    public class OrderApprovalUpdateDTO
    {
            [Required]
            public OrderApprovalStatus ApprovalStatus { get; set; }


        public string BranchName { get; set; }
        public string ShopName { get; set; }



        public string? RejectionReason { get; set; } // Required if rejected
        
    }
}
