using TailorAPI.DTO;
using TailorAPI.Models;
using TailorAPI.Repositories;
using System.Threading.Tasks;
using TailorAPI.Services.Interface;

public class AuthService : IAuthService
{
    private readonly UserRepository _userRepository;    
    private readonly JwtService _jwtService;

    public AuthService(UserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string?> AuthenticateUserAsync(string email, string password)
    {
        var user = (await _userRepository.GetAllUsersAsync())
            .FirstOrDefault(u => u.Email == email);

        if (user == null || user.IsDeleted)
        {
            Console.WriteLine($"User not found or deleted: {email}");
            return null;
        }

        if (string.IsNullOrEmpty(user.PasswordHash) || !user.PasswordHash.StartsWith("$2"))
        {
            Console.WriteLine($"Invalid password hash format for user: {email}");
            return null;
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!isPasswordValid)
        {
            Console.WriteLine($"Invalid password for user: {email}");
            return null;
        }

        // Add null check for Role
        if (user.Role == null)
        {
            Console.WriteLine($"No role assigned for user: {email}");
            return null;
        }

        return _jwtService.GenerateToken(user.UserID.ToString(), user.Role.RoleName);
    }

    public string HashPassword(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }
}
