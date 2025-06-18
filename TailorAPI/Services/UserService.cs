using Microsoft.AspNetCore.Identity;
using TailorAPI.DTO;
using TailorAPI.Repositories;
using TailorAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TailorAPI.Services.Interface;
using TailorAPI.Services;


public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly JwtService _JwtService;
    private readonly IShopService _shopService;
    private readonly IBranchService _branchService;
    private readonly BranchRepository _branchRepository;
    private readonly ShopRepository _shopRepository;



    public UserService(UserRepository userRepository,JwtService jwtService,IShopService shopService, IBranchService branchService,BranchRepository branchRepository,ShopRepository shopRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher<User>();
        _JwtService = jwtService;
        _shopService = shopService;
        _branchService = branchService;
        _branchRepository = branchRepository;
        _shopRepository = shopRepository;

    }




    public async Task<UserResponseDto?> RegisterUserAsync(UserRequestDto userDto)
    {
        try
        {
            if (userDto == null) throw new ArgumentNullException(nameof(userDto));

            if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                return null;

            var users = await _userRepository.GetAllUsersAsync();
            if (users.Any(u => u.Email == userDto.Email)) return null;

            var role = await _userRepository.GetRoleByNameAsync(userDto.RoleName);
            if (role == null) return null;

            // Step 1: Create User without shop/branch
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                MobileNo = userDto.MobileNo,
                Address = userDto.Address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                RoleID = role.RoleID
            };

            await _userRepository.CreateUserAsync(user); // Save to get user.UserID

            int shopId = 0;
            int branchId = 0;

            // Step 2: If Admin, create Shop and HEAD Branch
            if (userDto.RoleName == "Admin")
            {
                if (string.IsNullOrEmpty(userDto.ShopName) || string.IsNullOrEmpty(userDto.ShopLocation))
                    return null;

                var shop = new Shop
                {
                    ShopName = userDto.ShopName,
                    Location = userDto.ShopLocation,
                    CreatedByUserId = user.UserID,
                    CreatedByUserName = user.Name
                };
                await _shopRepository.CreateShopAsync(shop);
                shopId = shop.ShopId;

                var headBranch = await _branchService.CreateHeadBranchForShopAsync(shop);
                branchId = headBranch.BranchId;

                // Step 3: Update user with shop and branch info
                user.ShopId = shopId;
                user.BranchId = branchId;
                await _userRepository.UpdateUserAsync(user);
            }

            return new UserResponseDto
            {
                Name = user.Name,
                Email = user.Email,
                MobileNo = user.MobileNo,
                Address = user.Address,
                RoleName = role.RoleName,
                UserStatus = user.UserStatus.ToString(),
                IsVerified = user.IsVerified
            };
        }
        catch (Exception ex)
        {
            // Log the error if needed
            return null;
        }
    }



    public async Task<UserResponseDto?> AuthenticateUserAsync(string email, string password)
    {
        var user = (await _userRepository.GetAllUsersAsync())
            .FirstOrDefault(u => u.Email == email);

        if (user == null || user.IsDeleted) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result != PasswordVerificationResult.Success) return null;

        return new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            MobileNo = user.MobileNo,
            Address = user.Address,
            RoleName = user.Role.RoleName,
            UserStatus = user.UserStatus.ToString(),
            
        };
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(user => new UserResponseDto
        {
            UserID = user.UserID,
            Name = user.Name,
            Email = user.Email,
            MobileNo = user.MobileNo,
            Address = user.Address,
            RoleName = user.Role.RoleName,
            UserStatus = user.UserStatus.ToString()
        }).ToList();
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        return await _userRepository.DeleteUserAsync(userId);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return null;

        return new UserResponseDto
        {
            Name = user.Name,
            Email = user.Email,
            MobileNo = user.MobileNo,
            Address = user.Address,
            RoleName = user.Role.RoleName,
            UserStatus = user.UserStatus.ToString()
        };
    }

    public async Task<UserResponseDto?> UpdateUserAsync(int id, UserRequestDto userDto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null) return null;

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;
        existingUser.MobileNo = userDto.MobileNo;
        existingUser.Address = userDto.Address;

        await _userRepository.UpdateUserAsync(existingUser);

        return new UserResponseDto
        {
            Name = existingUser.Name,
            Email = existingUser.Email,
            MobileNo = existingUser.MobileNo,
            Address = existingUser.Address,
            RoleName = existingUser.Role.RoleName,
            UserStatus = existingUser.UserStatus.ToString()
        };
    }
}
