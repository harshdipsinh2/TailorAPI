﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TailorAPI.Models
{
    public class FabricStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockID { get; set; }

        [ForeignKey("FabricType")]
        public int FabricTypeID { get; set; }
        public FabricType FabricType { get; set; }

        [Required]
        public decimal StockIn { get; set; }

        [Required]
        public decimal StockUse { get; set; }

        [Required]
        public DateTime StockAddDate { get; set; }
    }
}
