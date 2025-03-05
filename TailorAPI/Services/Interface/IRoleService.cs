using TailorAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TailorAPI.Services.Interface
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<List<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<bool> UpdateRoleAsync(int roleId, string newRoleName);
        Task<bool> DeleteRoleAsync(int roleId);
    }
}
