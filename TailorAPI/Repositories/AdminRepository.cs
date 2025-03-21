using Microsoft.EntityFrameworkCore;
using TailorAPI.Models;

namespace TailorAPI.Repositories
{
    public class AdminRepository
    {
        private readonly TailorDbContext _context;

        public AdminRepository(TailorDbContext context)
        {
            _context = context;
        }

        // Get all Admins
        public async Task<List<User>> GetAllAdminsAsync()
        {
            return await _context.Users
                .Where(u => u.RoleID == 1) // Admin Role ID
                .ToListAsync();
        }

        // Get Admin by ID
        public async Task<User?> GetAdminByIdAsync(int adminId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == adminId && u.RoleID == 1);
        }

        // Add New Admin
        public async Task AddAdminAsync(User newAdmin)
        {
            await _context.Users.AddAsync(newAdmin);
            await _context.SaveChangesAsync();
        }

        // Update Admin
        public async Task UpdateAdminAsync(User admin)
        {
            _context.Users.Update(admin);
            await _context.SaveChangesAsync();
        }

        // Delete Admin
        public async Task DeleteAdminAsync(int adminId)
        {
            var admin = await _context.Users
                .FirstOrDefaultAsync(u => u.UserID == adminId && u.RoleID == 1);

            if (admin != null)
            {
                _context.Users.Remove(admin);
                await _context.SaveChangesAsync();
            }
        }
    }
}
