using System.Collections.Generic;
using System.Threading.Tasks;
using TailorAPI.Models;

namespace TailorAPI.Services.Interface
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<bool> UpdateRoleAsync(int roleId, string newRoleName);
        Task<bool> DeleteRoleAsync(int roleId);
    }
}