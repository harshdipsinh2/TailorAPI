using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;
using TailorAPI.Services.Interface;

namespace TailorAPI.Services
{
    public class RoleService : IRoleService
    {
        private readonly TailorDbContext _context;

        public RoleService(TailorDbContext context)
        {
            _context = context;
        }

        // Get All Roles
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        // Get Role by ID
        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        // Update Role
        public async Task<bool> UpdateRoleAsync(int roleId, string newRoleName)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            role.RoleName = newRoleName;
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete Role (Restrict deletion of seeded roles)
        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            // Prevent deletion of predefined roles
            if (roleId == 1 || roleId == 2 || roleId == 3) 
                return false; 

            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
