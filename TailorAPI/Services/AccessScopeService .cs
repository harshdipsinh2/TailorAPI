using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TailorAPI.Services.Interface;

public class AccessScopeService : IAccessScopeService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccessScopeService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public (string Role, int ShopId, int BranchId) GetUserScope()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User == null)
            return ("", 0, 0);

        var role = httpContext.User.FindFirst("roles")?.Value ?? "";
        var shopId = int.TryParse(httpContext.User.FindFirst("shopId")?.Value, out var sId) ? sId : 0;
        var branchId = int.TryParse(httpContext.User.FindFirst("branchId")?.Value, out var bId) ? bId : 0;

        return (role, shopId, branchId);
    }
    public bool IsSuperAdmin()
    {
        return GetUserScope().Role.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsAdmin()
    {
        return GetUserScope().Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsManager()
    {
        return GetUserScope().Role.Equals("Manager", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsTailor()
    {
        return GetUserScope().Role.Equals("Tailor", StringComparison.OrdinalIgnoreCase);
    }
       
}
