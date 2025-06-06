using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TailorAPI.DTO.RequestDTO;
using TailorAPI.Services.Interface;

public class DashboardService : IDashboardService
{
    private readonly TailorDbContext _context;

    public DashboardService(TailorDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDTO> GetDashboardSummaryAsync()
    {
        var result = await _context.Database.SqlQueryRaw<DashboardDTO>(
            "EXEC sp_GetTotalCounts123").ToListAsync();

        return result.FirstOrDefault() ?? new DashboardDTO();
    }
}
