using TailorAPI.Models;
using TailorAPI.DTO.RequestDTO;

namespace TailorAPI.Services.Interface
{
    public interface IPlanService
    {
        //StaticPlanType GetPlanForShop(int shopId);
        //bool IsBranchLimitExceeded(int shopId);
        //bool IsOrderLimitExceeded(int shopId, int branchId);
        //decimal GetPlanPrice(StaticPlanType planType);
        //Task<string> CreatePlanCheckoutSessionAsync(StaticPlanType planType, int durationInMonths);

        Task<IEnumerable<Plan>> GetAllPlansAsync();

        //Task<PlanCreateDTO> CreatePlanAsync(PlanCreateDTO dto);
        Task<string> CreatePlanCheckoutSessionAsync(int planId);
    }
}
