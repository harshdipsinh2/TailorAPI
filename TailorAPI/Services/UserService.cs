namespace TailorAPI.Services
{
    using Microsoft.AspNetCore.Identity;
    using TailorAPI.Models;
    using System.Threading.Tasks;
    using TailorAPI.Repositories;
    using TailorAPI.Services.Interface;
    using Microsoft.EntityFrameworkCore;
    using TailorAPI.DTO;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        // Register a new user
        public async Task<User> RegisterUserAsync(UserDto userDto)
        {
            // Check if the user already exists
            var users = await _userRepository.GetAllUsersAsync();
            if (users.Any(u => u.Email == userDto.Email))
                return null; // Prevent duplicate users

            // Find the role by RoleName
            var role = await _userRepository.GetRoleByNameAsync(userDto.RoleName);
            if (role == null) return null; // Handle case if role does not exist

            // Create user
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                MobileNo = userDto.MobileNo,
                Address = userDto.Address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                RoleID = role.RoleID // Assign role based on RoleName
            };

            await _userRepository.CreateUserAsync(user);
            return user;
        }

        // Authenticate user (Login)
        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = (await _userRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == email);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        // Get all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        // Get user by ID
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        // Update user
        public async Task<User?> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        // Delete user
        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }
    }
}