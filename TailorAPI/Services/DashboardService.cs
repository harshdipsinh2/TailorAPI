//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;

//public class DashboardService : IDashboardService
//{
//    private readonly ApplicationDbContext _context;

//    public DashboardService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync()
//    {
//        var result = await _context.Database.SqlQueryRaw<DashboardSummaryDTO>(
//            "EXEC sp_GetTotalCounts").ToListAsync();

//        return result.FirstOrDefault() ?? new DashboardSummaryDTO();
//    }
//}
