using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.Models;
using TailorAPI.Repositories;
using TailorAPI.Services.Interface;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using TailorAPI.Services;

public class AuthService : IAuthService
{
    private readonly UserRepository _userRepository;
    private readonly JwtService _jwtService;
    private readonly IBranchService _branchService;

    public AuthService(UserRepository userRepository,IBranchService branchService, JwtService jwtService)
    {
        _userRepository = userRepository;
        _branchService = branchService;
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
        return _jwtService.GenerateToken(
            user.UserID.ToString(),
            user.Role.RoleName,
            user.ShopId ?? 0,
            user.BranchId ?? 0
        );


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
            UserStatus = UserStatus.Available
        };

        if (request.RoleName == "Admin")
        {
            if (string.IsNullOrWhiteSpace(request.ShopName) || string.IsNullOrWhiteSpace(request.ShopLocation))
            {
                return new BadRequestObjectResult(new { Message = "ShopName and ShopLocation are required for Admin registration." });
            }

            // ✅ Save user first
            await _userRepository.CreateUserAsync(user);

            // ✅ Create Shop with user reference
            var shop = new Shop
            {
                ShopName = request.ShopName,
                Location = request.ShopLocation,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = user.UserID,
                CreatedByUserName = request.Name
            };

            await _userRepository.CreateShopAsync(shop);

            // ✅ Create Head Branch
            var headBranch = await _branchService.CreateHeadBranchForShopAsync(shop);

            // ✅ Update user with ShopId & BranchId
            user.ShopId = shop.ShopId;
            user.BranchId = headBranch.BranchId;

            await _userRepository.UpdateUserAsync(user); // You must have this method
        }
        else if (request.RoleName == "Manager" || request.RoleName == "Tailor")
        {
            if (request.ShopId == null || request.BranchId == null)
            {
                return new BadRequestObjectResult(new { Message = "ShopId and BranchId are required for Manager and Tailor registration." });
            }

            user.ShopId = request.ShopId;
            user.BranchId = request.BranchId;

            await _userRepository.CreateUserAsync(user);
        }
        else
        {
            return new BadRequestObjectResult(new { Message = "Unsupported role specified." });
        }

        return new OkObjectResult(new { Message = "User registered successfully." });
    }


}
