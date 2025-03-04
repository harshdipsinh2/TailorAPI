namespace TailorAPI.Repositories
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using TailorAPI.Models;

    public class UserRepository
    {
        private readonly TailorDbContext _context;

        public UserRepository(TailorDbContext context)
        {
            _context = context;
        }

        // Create a new user
        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }



        // Get a user by ID
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.Include(u => u.Role)
                                       .FirstOrDefaultAsync(u => u.UserID == userId);
        }

        // Get all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        // Update user
        public async Task<User?> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserID);
            if (existingUser == null) return null;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.MobileNo = user.MobileNo;
            existingUser.Address = user.Address;
            existingUser.RoleID = user.RoleID; // Update Role

            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Delete user
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
