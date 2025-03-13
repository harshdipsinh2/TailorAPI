using Microsoft.AspNetCore.Identity;
using TailorAPI.DTO;
using TailorAPI.Repositories;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User> RegisterUserAsync(UserDto userDto)
    {
        var users = await _userRepository.GetAllUsersAsync();
        if (users.Any(u => u.Email == userDto.Email))
            return null; // Prevent duplicate users

        var role = await _userRepository.GetRoleByNameAsync(userDto.RoleName);
        if (role == null) return null; // Handle case if role does not exist

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            MobileNo = userDto.MobileNo,
            Address = userDto.Address,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            RoleID = role.RoleID
        };

        await _userRepository.CreateUserAsync(user);
        return user;
    }

    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = (await _userRepository.GetAllUsersAsync()).FirstOrDefault(u => u.Email == email);
        if (user == null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success ? user : null;
    }

    public async Task<List<User>> GetAllUsersAsync() => await _userRepository.GetAllUsersAsync();

    public async Task<User?> GetUserByIdAsync(int userId) => await _userRepository.GetUserByIdAsync(userId);

    public async Task<User?> UpdateUserAsync(User user) => await _userRepository.UpdateUserAsync(user);

    public async Task<bool> DeleteUserAsync(int userId) => await _userRepository.DeleteUserAsync(userId);
}
