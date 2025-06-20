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
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<string?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
            return user == null ? null : user.Name;
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

        // Get a user by ID (Exclude deleted users)
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == userId && !u.IsDeleted);
        }

        // Get all users (Exclude deleted users)
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        // Update user
        public async Task<User?> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserID);
            if (existingUser == null || existingUser.IsDeleted) return null;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.MobileNo = user.MobileNo;
            existingUser.Address = user.Address;
            existingUser.RoleID = user.RoleID;


            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return existingUser;
        }

        // Soft delete user (Set IsDeleted = true)
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.IsDeleted) return false;

            user.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task CreateShopAsync(Shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
        }
    }
}
