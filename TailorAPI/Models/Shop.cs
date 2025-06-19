using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TailorAPI.Models
{
    public class Shop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShopId { get; set; }

        public string ShopName { get; set; }
        public string Location { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User? CreatedByUser { get; set; }

        public string? CreatedByUserName { get; set; }

        public ICollection<Branch> Branches { get; set; }

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
