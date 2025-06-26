using TailorAPI.Models;

namespace TailorAPI.Services.Interface
{
    public interface IPlanService
    {
        //StaticPlanType GetPlanForShop(int shopId);
        //bool IsBranchLimitExceeded(int shopId);
        //bool IsOrderLimitExceeded(int shopId, int branchId);
        //decimal GetPlanPrice(StaticPlanType planType);
        //Task<string> CreatePlanCheckoutSessionAsync(StaticPlanType planType, int durationInMonths);

        Task<string> CreatePlanCheckoutSessionAsync(int planId);
    }
}
