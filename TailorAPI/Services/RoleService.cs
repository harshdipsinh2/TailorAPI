using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (await _context.Roles.AnyAsync(r => r.RoleName == roleName))
                return false; // Role already exists

            var role = new Role { RoleName = roleName };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<bool> UpdateRoleAsync(int roleId, string newRoleName)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            role.RoleName = newRoleName;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
