namespace TailorAPI.DTO
{
    public class OrderRequestDto
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }

}
