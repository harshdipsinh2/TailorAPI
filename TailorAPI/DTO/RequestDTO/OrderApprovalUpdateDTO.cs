using System.ComponentModel.DataAnnotations;
using TailorAPI.Models;

namespace TailorAPI.DTO.RequestDTO
{
    public class OrderApprovalUpdateDTO
    {
             [Required]
            public int UserID { get; set; } // The tailor who is approving/rejecting
            [Required]
            public OrderApprovalStatus ApprovalStatus { get; set; }
            public string? RejectionReason { get; set; } // Required if rejected
        
    }
}
