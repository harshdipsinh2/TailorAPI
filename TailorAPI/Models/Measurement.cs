using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailorAPI.Models
{
    public class Measurement
    {
        [Key]
        public int MeasurementID { get; set; }

        public float Chest { get; set; }
        public float Waist { get; set; }
        public float Hip { get; set; }
        public float Shoulder { get; set; }
        public float SleeveLength { get; set; }
        public float TrouserLength { get; set; }
        public float Inseam { get; set; }
        public float Thigh { get; set; }
        public float Neck { get; set; }
        public float Sleeve { get; set; }
        public float Arms { get; set; }
        public float Bicep { get; set; }
        public float Forearm { get; set; }
        public float Wrist { get; set; }
        public float Ankle { get; set; }
        public float Calf { get; set; }
        public float UpperBodyMeasurement { get; set; }
        public float LowerBodyMeasurement { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }
    }
}