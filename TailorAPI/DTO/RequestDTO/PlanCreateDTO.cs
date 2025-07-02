namespace TailorAPI.DTO.RequestDTO
{
    public class PlanCreateDTO
    {
        public string Name { get; set; } = null!;
        public int MaxBranches { get; set; }
        public int MaxOrders { get; set; }
        public decimal PricePerMonth { get; set; }
        public string StripeProductId { get; set; } = null!;
        public string StripePriceId { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
