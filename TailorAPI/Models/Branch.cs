using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TailorAPI.Models
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public string Location { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // ✅ New field

        [ForeignKey("Shop")]
        public int ShopId { get; set; }

        public Shop Shop { get; set; } // Navigation property

        public ICollection<User> Users { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<FabricStock> FabricStocks { get; set; }
        public ICollection<FabricType> FabricTypes { get; set; }
        public ICollection<Measurement> Measurements { get; set; }
        public ICollection<TwilioSms> TwilioSmss { get; set; }
    }
}
