using Microsoft.AspNetCore.Mvc;
using TailorAPI.DTO;
using TailorAPI.DTO.RequestDTO;
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
    private readonly IOtpVerificationService _otpVerificationService;

    public AuthService(
        UserRepository userRepository,
        IBranchService branchService,
        JwtService jwtService,
        IOtpVerificationService otpVerificationService)
    {
        _userRepository = userRepository;
        _branchService = branchService;
        _jwtService = jwtService;
        _otpVerificationService = otpVerificationService;
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

        if (!user.IsVerified)
        {
            Console.WriteLine($"User not verified: {email}");
            return null;
        }

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

    // ✅ New OTP-based Registration
    public async Task<IActionResult> RegisterUserWithOtpAsync(UserRequestDto request)
    {
        var existingUser = (await _userRepository.GetAllUsersAsync())
            .FirstOrDefault(u => u.Email == request.Email);
        if (existingUser != null)
        {
            return new ConflictObjectResult(new { Message = "Email already exists." });
        }

        var role = await _userRepository.GetRoleByNameAsync(request.RoleName);
        if (role == null)
        {
            return new BadRequestObjectResult(new { Message = "Invalid role specified." });
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            MobileNo = request.MobileNo,
            Address = request.Address,
            PasswordHash = HashPassword(request.Password),
            RoleID = role.RoleID,
            UserStatus = UserStatus.Available,
            IsVerified = false
        };

        if (request.RoleName == "Admin")
        {
            if (string.IsNullOrWhiteSpace(request.ShopName) || string.IsNullOrWhiteSpace(request.ShopLocation))
            {
                return new BadRequestObjectResult(new { Message = "ShopName and ShopLocation are required for Admin registration." });
            }

            // Step 1: Save user
            await _userRepository.CreateUserAsync(user);

            // Step 2: Create shop
            var shop = new Shop
            {
                ShopName = request.ShopName,
                Location = request.ShopLocation,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = user.UserID,
                CreatedByUserName = request.Name
            };
            await _userRepository.CreateShopAsync(shop);

            // Step 3: Create Head Branch
            var headBranch = await _branchService.CreateHeadBranchForShopAsync(shop);

            // Step 4: Update user with ShopId and BranchId
            user.ShopId = shop.ShopId;
            user.BranchId = headBranch.BranchId;

            await _userRepository.UpdateUserAsync(user);
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

        // ✅ Send OTP
        var otpSent = await _otpVerificationService.GenerateAndSendOtpAsync(request.Email);
        if (!otpSent)
        {
            return new BadRequestObjectResult(new { Message = "Failed to send OTP email." });
        }

        return new OkObjectResult(new { Message = "OTP sent to email. Please verify to complete registration." });
    }


    public async Task<bool> VerifyOtpAndActivateUserAsync(OtpVerificationsRequestDTO dto)
    {
        return await _otpVerificationService.VerifyOtpAsync(dto);
    }

    // ✅ Original registration (optional, you can remove if unused)
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

            await _userRepository.CreateUserAsync(user);

            var shop = new Shop
            {
                ShopName = request.ShopName,
                Location = request.ShopLocation,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = user.UserID,
                CreatedByUserName = request.Name
            };

            await _userRepository.CreateShopAsync(shop);
            var headBranch = await _branchService.CreateHeadBranchForShopAsync(shop);
            user.ShopId = shop.ShopId;
            user.BranchId = headBranch.BranchId;

            await _userRepository.UpdateUserAsync(user);
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
