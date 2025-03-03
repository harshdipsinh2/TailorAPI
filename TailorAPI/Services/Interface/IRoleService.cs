using Microsoft.AspNetCore.Identity;

namespace TailorAPI.Services.Interface
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole?> GetRoleByIdAsync(string roleId);
        Task<bool> UpdateRoleAsync(string roleId, string newRoleName);
        Task<bool> DeleteRoleAsync(string roleId);
    }

}
