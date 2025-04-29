//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace TailorAPI.Models
//{
//    public class Subscription
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
//        public int SubscriptionID { get; set; }

//        [ForeignKey("User")]
//        public int UserID { get; set; }
//        public User User { get; set; } // Navigation property

//        [Required]
//        public string PlanType { get; set; }
//        public DateTime TrialEndTime { get; set; } 
//        public DateTime? StartDate { get; set; }
//        public DateTime? ExpirationDate { get; set; }


//        public string Status { get; set; } = "Active";

//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



//    }
//}

