using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.Models;
using TailorAPI.Repositories;
using TailorAPI.Services.Interface;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

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

        if (user == null)
        {
            Console.WriteLine($"User not found: {email}");
            return null;
        }

        if (user.IsDeleted)
        {
            Console.WriteLine($"User is deleted: {email}");
            return null;
        }

        if (string.IsNullOrWhiteSpace(user.PasswordHash) || !user.PasswordHash.StartsWith("$2"))
        {
            Console.WriteLine($"Invalid password hash format for user: {email}");
            return null;
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            Console.WriteLine($"Password verification failed for user: {email}");
            return null;
        }

        Console.WriteLine($"User authenticated successfully: {email}");
        return _jwtService.GenerateToken(user.UserID.ToString(), user.Role.RoleName);
    }


    public string HashPassword(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    // ✅ New RegisterUserAsync Method
    public async Task<IActionResult> RegisterUserAsync(UserRequestDto request)
    {
        var users = await _userRepository.GetAllUsersAsync();
        if (users.Any(u => u.Email == request.Email))
        {
            return new ConflictObjectResult(new { Message = "Email already exists." });
        }

        var role = await _userRepository.GetRoleByNameAsync(request.RoleName);
        if (role == null)
        {
            return new ConflictObjectResult(new { Message = "Invalid role specified." });
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            MobileNo = request.MobileNo,
            Address = request.Address,
            PasswordHash = HashPassword(request.Password),
            RoleID = role.RoleID,
            UserStatus = UserStatus.Available // Default status as Available
        };

        await _userRepository.CreateUserAsync(user);

        return new OkObjectResult(new { Message = "User registered successfully." });
    }
}
