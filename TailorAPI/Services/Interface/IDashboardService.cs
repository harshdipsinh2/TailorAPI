using TailorAPI.DTO.RequestDTO;

namespace TailorAPI.Services.Interface
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardSummaryAsync();
    }

}
