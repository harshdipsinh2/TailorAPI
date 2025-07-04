namespace TailorAPI.DTO.ResponseDTO
{
    public class MeasurementResponseDTO
    {

        public int MeasurementID { get; set; }

        public int CustomerId { get; set; }

        public string FullName { get; set; }


        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }


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

    }
}