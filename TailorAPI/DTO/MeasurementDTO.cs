using System.Text.Json.Serialization;

public class MeasurementDTO
{
    public int MeasurementID { get; set; }

    [JsonIgnore]
    public int CustomerID { get; set; } // ✅ Required to associate measurement with a customer
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
}
