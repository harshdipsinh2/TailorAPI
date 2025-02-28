using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Measurement
{
    [Key] // ✅ Primary Key (Only One!)
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MeasurementID { get; set; }

    [ForeignKey("Customer")]
    public int CustomerID { get; set; }

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

    public bool IsDeleted { get; set; } = false; // ✅ Soft delete flag

    // Navigation Property (Ignored in API Response)
    [JsonIgnore]
    public Customer Customer { get; set; }
}
